using MusicHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
        public interface IPostReportRepository
        {
            Task AddAsync(PostReport report, CancellationToken ct);
            Task<int> CountAsync(CancellationToken ct);
            Task<List<PostReport>> GetPagedAsync(int skip, int take, CancellationToken ct);
        }
}
