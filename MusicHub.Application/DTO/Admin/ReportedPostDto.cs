using MusicHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO.Admin
{
    public sealed class ReportedPostDto
    {
        public Guid ReportId { get; set; }
        public Guid PostId { get; set; }
        public Guid ReportedByUserId { get; set; }
        public ReportReason Reason { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
