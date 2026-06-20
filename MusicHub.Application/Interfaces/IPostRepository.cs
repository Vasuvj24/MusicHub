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
        Task<(int total, List<Post> items)>GetPagedAsync(int skip,int take,CancellationToken ct);
        Task<Post?> GetByIdIncludingDeletedAsync(Guid postId,CancellationToken ct);
        Task<List<Post>> SearchPostsAsync(string term);
    }
}
