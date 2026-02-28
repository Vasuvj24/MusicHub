using MusicHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Entities
{
    public sealed class PostReport
    {
        public Guid Id { get; private set;  } = Guid.NewGuid(); 
        public Guid PostId { get; private set; }
        public Guid ReportedByUserId { get; private set; }
        public ReportReason Reason { get; private set; }
        public string? Note { get; private set; }

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        private PostReport() { } // EF
        public PostReport(Guid postId, Guid reportedByUserId, ReportReason reason, string? note)
        {
            PostId = postId;
            ReportedByUserId = reportedByUserId;
            Reason = reason;
            Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
        }
    }
}
