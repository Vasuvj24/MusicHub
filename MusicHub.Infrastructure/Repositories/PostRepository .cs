using Microsoft.EntityFrameworkCore;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
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

        public async Task<(int total, List<Post> items)> GetPagedAsync(int skip, int take, CancellationToken ct)
        {
            var query = _db.Posts.OrderByDescending(p => p.CreatedAtUtc);
            var total = await query.CountAsync(ct);
            var items = await query.Skip(skip).Take(take).ToListAsync(ct);
            return (total, items);
        }
    }
}
