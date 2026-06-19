using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO
{
    public class NotificationResponseDto
    {
        public Guid Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
