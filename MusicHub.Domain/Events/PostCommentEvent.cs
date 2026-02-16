using MusicHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Events
{
    public class PostCommentEvent : DomainEvent
    {
        public Guid PostId { get; }
        public Guid CommentedByUserId { get; }

        public PostCommentEvent(Guid postId, Guid commentedByUserId)
        {
            PostId = postId;
            CommentedByUserId = commentedByUserId;
        }
    }
}
