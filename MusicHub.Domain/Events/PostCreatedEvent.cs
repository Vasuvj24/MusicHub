using MusicHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Events
{
    public sealed class PostCreatedEvent : DomainEvent
    {
        public Guid PostId { get; }
        public Guid UserId { get; }
        public PostCreatedEvent(Guid postId, Guid userId)
        {
            PostId = postId;
            UserId = userId;
        }
    }
}
