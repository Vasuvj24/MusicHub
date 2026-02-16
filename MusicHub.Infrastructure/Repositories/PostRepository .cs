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
        }
        public async Task AddAsync(Post post,CancellationToken cts)
        {
            await _db.Posts.AddAsync(post,cts);
        }
        public async Task<Post?> GetByIdAsync(Guid postId,CancellationToken cts)
        {
            return await _db.Posts.FirstOrDefaultAsync(p => p.Id == postId,cts);
        }
        public async Task<List<Post>> GetLatestAsync(int take, CancellationToken cts)
        {
            return await _db.Posts.OrderByDescending(p => p.CreatedAtUtc).Take(take).ToListAsync();
        }
    }
}
