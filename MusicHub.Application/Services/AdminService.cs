using MusicHub.Application.DTO.Admin;
using MusicHub.Application.DTO.Common;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using MusicHub.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Services
{
    public sealed class AdminService
    {
        private readonly IPostRepository _posts;
        private readonly IPostReportRepository _reports;
        private readonly IUnitOfWork _uow;
        public AdminService(IPostRepository posts, IPostReportRepository reports, IUnitOfWork uow)
        {
            _posts = posts;
            _reports = reports;
            _uow = uow;
        }

        public async Task ReportPostAsync(Guid currentUserId, Guid postId, ReportReason reason, string? note, CancellationToken ct)
        {
            var post = await _posts.GetByIdAsync(postId, ct) ?? throw new KeyNotFoundException("Post not found.");
            // even if deleted later, keep reports.

            await _reports.AddAsync(new PostReport(postId, currentUserId, reason, note), ct);
            await _uow.SaveChangesAsync();
        }

        public async Task<PagedResult<ReportedPostDto>> GetReportsAsync(PagedRequest req, CancellationToken ct)
        {
            req.Normalize();

            var total = await _reports.CountAsync(ct);
            var skip = (req.Page - 1) * req.PageSize;

            var items = await _reports.GetPagedAsync(skip, req.PageSize, ct);

            return new PagedResult<ReportedPostDto>
            {
                Page = req.Page,
                PageSize = req.PageSize,
                Total = total,
                Items = items.Select(r => new ReportedPostDto
                {
                    ReportId = r.Id,
                    PostId = r.PostId,
                    ReportedByUserId = r.ReportedByUserId,
                    Reason = r.Reason,
                    Note = r.Note,
                    CreatedAtUtc = r.CreatedAtUtc
                }).ToList()
            };
        }

        public async Task SoftDeletePostAsync(Guid postId, CancellationToken ct)
        {
            var post = await _posts.GetByIdIncludingDeletedAsync(postId, ct)
                       ?? throw new KeyNotFoundException("Post not found.");

            post.SoftDelete();
            await _uow.SaveChangesAsync();
        }
    }
}
