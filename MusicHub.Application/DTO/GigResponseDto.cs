using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO
{
    public class GigResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid CreatorId { get; set; }
        public DateTime ScheduledAtUtc { get;  set; }

    }
}
