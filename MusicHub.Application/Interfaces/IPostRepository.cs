using MusicHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
    public interface IPostRepository
    {
        Task AddAsync(Post post, CancellationToken cts);
        Task<Post?> GetByIdAsync(Guid PostId,CancellationToken cts);
        Task<List<Post>> GetLatestAsync(int take,CancellationToken cts);
    }
}
