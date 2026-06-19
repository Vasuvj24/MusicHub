using Microsoft.EntityFrameworkCore;
using MusicHub.Domain.Common;
using MusicHub.Domain.Entities;
using MusicHub.Domain.Users;
using MusicHub.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly EventDispatcher _dispatcher;
        //configuring the configuration via options and using base class constructor to do that and also setting dispatcher
        public AppDbContext(DbContextOptions<AppDbContext> options,EventDispatcher dispatcher) : base(options)
        {
            _dispatcher = dispatcher;
        }
        //creating a user table in db
        //set<> is meathod to get the user object that helps qeury the dbfrom the db
        //return object to the refecence of dbset of type t
        public DbSet<User> Users => Set<User>();
        public DbSet<Gig> Gigs => Set<Gig>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<PostReport> PostReports => Set<PostReport>();
        public DbSet<UserProfile> Profiles => Set<UserProfile>();
        public DbSet<Notification> Notifications { get; set; }

        //essentially for making the blueprint for change tracker also used by qyeru translation , save changes, migrations 
        //runs on migration and also in once per app lifecycle
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //overriding the actual model 
            modelBuilder.Entity<Post>(b =>
            {
                b.HasKey(x=>x.Id);
                b.Property(x => x.Caption).HasMaxLength(500);
                b.Property(x => x.MediaUrl).HasMaxLength(1000).IsRequired();
                b.HasQueryFilter(p => !p.IsDeleted);
                //we enforce post is aggregate root and follow domain driven event 
                b.OwnsMany(p => p.Likes, lb =>
                {
                    //owned by posts
                    lb.WithOwner().HasForeignKey("PostId");
                    lb.Property<Guid>("PostId");
                    lb.Property<Guid>("UserId");
                    lb.HasKey("PostId", "UserId"); // unique like per user per post
                    lb.ToTable("PostLikes");
                });
                b.Navigation(p => p.Likes).HasField("_likes");
                b.OwnsMany(p => p.Comments, cb =>
                {
                    cb.WithOwner().HasForeignKey("PostId");
                    cb.Property<Guid>("PostId");
                    cb.Property<Guid>("Id");
                    cb.HasKey("Id");
                    cb.Property<string>("Text").HasMaxLength(500).IsRequired();
                    cb.ToTable("PostComments");
                });
                b.Navigation(p => p.Comments).HasField("_comments");
            });
            modelBuilder.Entity<Gig>(g =>
            {
                //setting primary key
                g.HasKey(x=> x.Id);
                g.Property(x => x.Title).HasMaxLength(200).IsRequired();
                g.Property(x => x.Description).HasMaxLength(2000);
                g.OwnsMany(g => g.Members, gig =>
                {
                    gig.WithOwner().HasForeignKey("GigId");
                    gig.Property<Guid>("GigId");
                    gig.Property<Guid>("UserId");
                    //making this composite primary key
                    gig.HasKey("GigId", "UserId"); // unique per gig per user
                    gig.ToTable("GigMembers");
                });
                //members have backing field _members
                g.Navigation(g => g.Members).HasField("_members");
            });
            modelBuilder.Entity<UserProfile>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.UserId).IsUnique();

                b.Property(x => x.DisplayName).HasMaxLength(100);
                b.Property(x => x.Bio).HasMaxLength(1000);
                b.Property(x => x.City).HasMaxLength(100);
                b.Property(x => x.Genres).HasMaxLength(200);

                b.OwnsMany(t=> t.Services, sb =>
                {
                    sb.WithOwner().HasForeignKey("ProfileId");
                    sb.Property<Guid>("ProfileId");
                    sb.Property<Guid>("Id");
                    sb.HasKey("Id");
                    sb.Property<string>("Title").HasMaxLength(200).IsRequired();
                    sb.Property<string>("Description").HasMaxLength(2000);
                    sb.ToTable("ServiceListings");
                });
            });
            modelBuilder.Entity<RefreshToken>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.UserId);
                b.HasIndex(x => x.TokenHash).IsUnique();
                b.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
            });

            modelBuilder.Entity<PostReport>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.PostId);
                b.HasIndex(x => x.ReportedByUserId);
                b.Property(x => x.Note).HasMaxLength(1000);
            });

        }
        //overriding the meathid for logging for whenever the operation is complete
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntries = ChangeTracker.Entries<BaseEntity>().Where(x => x.Entity.DomainEvents.Any()).ToList();
            foreach (var e in this.ChangeTracker.Entries())
            {
                Console.WriteLine($"{e.Metadata.Name} => {e.State}");
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            foreach (var entity in domainEntries)
            {
                foreach (var domainevent in entity.Entity.DomainEvents)
                {
                    await _dispatcher.Dispatch(domainevent);
                }
                //clearing this for particular life cycle of save changes 
                entity.Entity.ClearDomainEvents();
            }
            //int represents how many rows are affected
            return result;
        }

    }
}
