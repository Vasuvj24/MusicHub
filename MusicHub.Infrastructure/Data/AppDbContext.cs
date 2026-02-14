using Microsoft.EntityFrameworkCore;
using MusicHub.Domain.Common;
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
        //configuring the configuration via options and usign base class constructor to do that and also setting dispatcher
        public AppDbContext(DbContextOptions<AppDbContext> options,EventDispatcher dispatcher) : base(options)
        {
            _dispatcher = dispatcher;
        }
        //property to get the user from the db
        public DbSet<User> Users => Set<User>();
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
                entity.Entity.ClearDomainEvents();
            }
            //int represents how many rows are affected
            return result;
        }

    }
}
