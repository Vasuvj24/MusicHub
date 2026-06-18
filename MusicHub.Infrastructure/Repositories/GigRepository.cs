using Microsoft.EntityFrameworkCore;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using MusicHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Repositories
{
    public sealed class GigRepository : IGigRepository
    {
        private readonly AppDbContext _db;

        public GigRepository(AppDbContext db)
        {
            _db = db;
        }
        public Task AddAsync(Gig gig,CancellationToken cts)
        {
            _db.Gigs.AddAsync(gig,cts);
            return Task.CompletedTask;
        }
        public async Task<List<Gig>> SearchGigsAsync(string term)
        {
            return await _db.Gigs
                .Where(x =>
                    x.Title.Contains(term) ||
                    x.Description.Contains(term))
                .OrderByDescending(x => x.ScheduledAtUtc)
                .ToListAsync();
        }
        public Task<Gig?> GetByIdAsync(Guid gigId, CancellationToken ct)
        {
            return _db.Gigs.FirstOrDefaultAsync(g => g.Id == gigId, ct);
        }
        public Task<List<Gig>> GetLatestAsync(int take, CancellationToken ct)
        {
            return _db.Gigs.OrderByDescending(g => g.ScheduledAtUtc).Take(take).ToListAsync(ct);
        }
    }
}
