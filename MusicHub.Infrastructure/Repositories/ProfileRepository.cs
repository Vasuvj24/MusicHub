using Microsoft.EntityFrameworkCore;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using MusicHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Repositories
{
    public sealed class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _db;
        public ProfileRepository(AppDbContext db)
        {
            _db = db;
        }
        public Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct)
        => _db.Profiles.FirstOrDefaultAsync(p => p.UserId == userId, ct);

        public Task AddAsync(UserProfile profile, CancellationToken ct)
            => _db.Profiles.AddAsync(profile, ct).AsTask();
    }
}
