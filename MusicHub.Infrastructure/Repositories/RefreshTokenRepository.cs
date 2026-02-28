using Microsoft.Identity.Client;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using MusicHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Repositories
{
    public sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _db;
        public RefreshTokenRepository(AppDbContext db)
        {
            _db = db;
        }
        public Task AddAsync(RefreshToken token,CancellationToken cts)
             => _db.RefreshTokens.AddAsync(token, cts).AsTask();
        public Task<RefreshToken> GetActiveByHashAsync(string tokenHash,CancellationToken cts)
            => _db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash && x.RevokedAtUtc == null && x.ExpiresAtUtc > DateTime.UtcNow, cts);

        public Task<List<RefreshToken>> GetActiveByUserAsync(Guid userId, CancellationToken cts)
        => _db.RefreshTokens.Where(x => x.UserId == userId && x.RevokedAtUtc == null && x.ExpiresAtUtc > DateTime.UtcNow).ToListAsync(cts);


    }
}
