using Microsoft.EntityFrameworkCore;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using MusicHub.Domain.Enums;
using MusicHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Repositories
{
    public sealed class PostRepository : IPostRepository
    {
        private readonly AppDbContext _db;
        public PostRepository(AppDbContext db)
        {
            _db = db;
            Console.WriteLine("Repo ctx: " + _db.GetHashCode());

        }
        public async Task AddAsync(Post post,CancellationToken cts)
        {
            await _db.Posts.AddAsync(post,cts);
        }
        public async Task<Post?> GetByIdAsync(Guid postId,CancellationToken cts)
        {
            return await _db.Posts.AsSplitQuery().Include(p=>p.Likes).Include(c=>c.Comments).FirstOrDefaultAsync(p => p.Id == postId,cts);
        }
        public async Task<List<Post>> SearchPostsAsync(string term)
        {
            return await _db.Posts
                .Where(x =>
                    x.Caption.Contains(term))
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync();
        }
        public async Task<List<Post>> GetLatestAsync(int take, CancellationToken cts)
        {
            return await _db.Posts.OrderByDescending(p => p.CreatedAtUtc).Take(take).ToListAsync();
        }
        public Task<Post?> GetByIdIncludingDeletedAsync(Guid postId, CancellationToken ct)
    => _db.Posts.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == postId, ct);

        public async Task<(int Total, List<Post> Posts)> GetPagedAsync(
    int page,
    int pageSize,
    InstrumentType? instrument,
    string sortBy,
    CancellationToken ct)
        {
            IQueryable<Post> query =
                _db.Posts
                .Include(x => x.Likes)
                .Include(x => x.Comments);

            if (instrument.HasValue)
            {
                query =
                    query.Where(x =>
                        x.Instrument == instrument.Value);
            }

            query =
                sortBy.ToLower() switch
                {
                    "likes" =>
                        query.OrderByDescending(x => x.Likes.Count),

                    _ =>
                        query.OrderByDescending(x => x.CreatedAtUtc)
                };

            var total =
                await query.CountAsync(ct);

            var posts =
                await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (total, posts);
        }
    }
}
