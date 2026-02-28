using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using MusicHub.Infrastructure.Data;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Repositories
{
    public sealed class PostReportRepository : IPostReportRepository
    {
        private readonly AppDbContext _db;

        public PostReportRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task AddAsync(PostReport report, CancellationToken cts)
            => _db.PostReports.AddAsync(report, cts).AsTask();

        public Task<int> CountAsync(CancellationToken ct)
            => _db.PostReports.CountAsync(ct);

        public Task<List<PostReport>> GetPagedAsync(int skip, int take, CancellationToken cts)
            => _db.PostReports
                .OrderByDescending(x => x.CreatedAtUtc)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cts);
    }
}
