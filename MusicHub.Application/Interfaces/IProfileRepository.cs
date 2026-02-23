using MusicHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
    public interface IProfileRepository
    {
        Task<UserProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct);
        Task AddAsync(UserProfile profile, CancellationToken ct);
    }
}
