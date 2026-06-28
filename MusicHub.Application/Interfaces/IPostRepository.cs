using MusicHub.Domain.Entities;
using MusicHub.Domain.Enums;
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
        Task<(int Total, List<Post> Posts)> GetPagedAsync(int page,int pageSize,InstrumentType? instrument,string sortBy,CancellationToken ct);
        Task<Post?> GetByIdIncludingDeletedAsync(Guid postId,CancellationToken ct);
        Task<List<Post>> SearchPostsAsync(string term);
    }
}
