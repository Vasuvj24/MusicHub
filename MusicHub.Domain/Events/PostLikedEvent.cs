using MusicHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Events
{
    public class PostLikedEvent : DomainEvent
    {
        public Guid PostId { get; }
        public Guid LikedByUserId { get; }
        public PostLikedEvent(Guid postId, Guid likedByUserId)
        {
            PostId = postId;
            LikedByUserId = likedByUserId;
        }
    }
}
