using MusicHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
    public interface IGigRepository
    {
        Task AddAsync(Gig gig, CancellationToken cts);
        Task<Gig?> GetByIdAsync(Guid gigId,CancellationToken cts);
        Task<List<Gig>> GetLatestAsync(int take,CancellationToken cts)   ;
        Task<(int total, List<Gig> items)>GetPagedAsync(int skip,int take,CancellationToken ct);
        Task<List<Gig>> SearchGigsAsync(string term);
    }
}
