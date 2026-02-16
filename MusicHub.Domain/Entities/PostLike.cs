using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Entities
{
    public class PostLike
    {
        public Guid PostId {  get; private set; }
        public Guid UserId {  get; private set; }
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        private PostLike() { }
        public PostLike(Guid postId, Guid userId)
        {
            PostId = postId;
            UserId = userId;
        }
    }
}
