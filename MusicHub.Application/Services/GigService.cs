using MusicHub.Application.DTO;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Services
{
    public sealed class GigService
    {
        private readonly IGigRepository _gigs;
        private readonly IUnitOfWork _uow;
        public GigService(IGigRepository gigs, IUnitOfWork uow)
        {
            _gigs = gigs;
            _uow = uow;
        }
        public async Task<Guid> CreateAsync(Guid currentUserId, CreateGigDto dto, CancellationToken ct)
        {
            var gig = new Gig(currentUserId, dto.Title, dto.Description ?? "", dto.ScheduledAtUtc);
            await _gigs.AddAsync(gig, ct);
            await _uow.SaveChangesAsync();
            return gig.Id;
        }
        public async Task ApplyAsync(Guid currentUserId, Guid gigId, ApplyToGigDto dto, CancellationToken ct)
        {
            var gig = await _gigs.GetByIdAsync(gigId, ct) ?? throw new KeyNotFoundException("Gig not found");
            gig.Apply(currentUserId, dto.Instrument);
            await _uow.SaveChangesAsync();
        }
        public async Task ApproveAsync(Guid currentUserId, Guid gigId, ApproveMemberDto dto, CancellationToken ct)
        {
            var gig = await _gigs.GetByIdAsync(gigId, ct) ?? throw new KeyNotFoundException("Gig not found");
            gig.ApproveMember(currentUserId, dto.MemberUserId);
            await _uow.SaveChangesAsync();
        }
        public async Task<List<Gig>> GetLatestAsync(int take, CancellationToken ct)
        {
            //at max 50 and at min 1
            take = Math.Clamp(take, 1, 50);
            return await _gigs.GetLatestAsync(take, ct);
        }
    }
}
