using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Entities
{
    public class PostComment
    {
        public Guid Id { get; private set; } 

        public Guid PostId { get; private set; }
        public Guid UserId { get; private set; }
        public string Text { get; private set; } = string.Empty;

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        private PostComment() { } // EF

        public PostComment(Guid userId, string text)
        {
            //Id = Guid.NewGuid();
            //PostId = postId;
            UserId = userId;
            Text = text;
        }
    }
}
