using Microsoft.EntityFrameworkCore;
using MusicHub.Domain.Common;
using MusicHub.Domain.Entities;
using MusicHub.Domain.Users;
using MusicHub.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Text;

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
        //set is property to get the user from the db
        //return object to the refecence of dbset of type t
        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        //essentially for making the blueprint for change tracker also used by qyeru translation , save changes, migrations 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //overriding the actual model 
            modelBuilder.Entity<Post>(b =>
            {
                b.HasKey(x=>x.Id);
                b.Property(x => x.Caption).HasMaxLength(500);
                b.Property(x => x.MediaUrl).HasMaxLength(1000).IsRequired();
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
        }
        //overriding the meathid for logging for whenever the operation is complete
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntries = ChangeTracker.Entries<BaseEntity>().Where(x => x.Entity.DomainEvents.Any()).ToList();
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
