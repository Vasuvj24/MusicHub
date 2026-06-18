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
        Task<List<Gig>> SearchGigsAsync(string term);
    }
}
