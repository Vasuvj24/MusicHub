using MusicHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
        public interface IRefreshTokenRepository
        {
            Task AddAsync(RefreshToken token, CancellationToken ct);
            Task<RefreshToken?> GetActiveByHashAsync(string tokenHash, CancellationToken ct);
            Task<List<RefreshToken>> GetActiveByUserAsync(Guid userId, CancellationToken ct);
        }
}
