using MusicHub.Application.DTO;
using MusicHub.Application.DTO.Common;
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
        private INotificationRepository _nr;
        public GigService(IGigRepository gigs, IUnitOfWork uow,INotificationRepository nr)
        {
            _gigs = gigs;
            _uow = uow;
            _nr = nr;
        }
        public async Task<List<GigResponseDto>>  SearchGigsAsync(string term)
        {
            var gigs = await _gigs.SearchGigsAsync(term);

            return gigs.Select(g => new GigResponseDto
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                CreatorId = g.CreatedByUserId,
                ScheduledAtUtc = g.ScheduledAtUtc
            }).ToList();
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
            await _nr.AddAsync(
                new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = gig.CreatedByUserId,
                    Message =
                        $"{dto.Instrument} player musician applied to your gig",
                    IsRead = false,
                    CreatedAtUtc = DateTime.UtcNow
                });
            await _uow.SaveChangesAsync();
        }
        public async Task<PagedResult<GigResponseDto>>GetPagedAsync(int page,int pageSize,CancellationToken ct)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);

            int skip = (page - 1) * pageSize;

            var result = await _gigs.GetPagedAsync(
                    skip,
                    pageSize,
                    ct);

            return new PagedResult<GigResponseDto>
            {
                Total = result.total,

                Items = result.items
                    .Select(g => new GigResponseDto
                    {
                        Id = g.Id,
                        Title = g.Title,
                        Description = g.Description,
                        CreatorId = g.CreatedByUserId,
                        ScheduledAtUtc = g.ScheduledAtUtc
                    })
                    .ToList()
            };
        }
        public async Task DeleteAsync(Guid currentUserId,Guid gigId,CancellationToken ct)
        {
            var gig =
                await _gigs.GetByIdAsync(
                    gigId,
                    ct)
                ?? throw new KeyNotFoundException(
                    "Gig not found");

            gig.SoftDelete(currentUserId);

            await _uow.SaveChangesAsync();
        }
        public async Task ApproveAsync(Guid currentUserId, Guid gigId, ApproveMemberDto dto, CancellationToken ct)
        {
            var gig = await _gigs.GetByIdAsync(gigId, ct) ?? throw new KeyNotFoundException("Gig not found");
            gig.ApproveMember(currentUserId, dto.MemberUserId);
            await _nr.AddAsync(
            new Notification
            {
                Id = Guid.NewGuid(),
                UserId = dto.MemberUserId,
                Message =
                    $"You were approved for gig '{gig.Title}'",
                IsRead = false,
                CreatedAtUtc = DateTime.UtcNow
            });
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
