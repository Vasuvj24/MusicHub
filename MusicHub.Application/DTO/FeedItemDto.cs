using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO
{
    public class FeedItemDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Caption { get; set; } = "";

        public string MediaUrl { get; set; } = "";

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
