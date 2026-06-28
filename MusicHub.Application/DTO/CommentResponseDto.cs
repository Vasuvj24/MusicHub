using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO
{
    public class CommentResponseDto
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string Text { get; set; } = "";

        public DateTime CreatedAtUtc { get; set; }
    }
}
