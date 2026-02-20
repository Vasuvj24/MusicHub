using MusicHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Events
{
    public sealed class GigMemberApprovedEvent : DomainEvent
    {
        public Guid GigId { get; }
        public Guid UserId { get; }
        public GigMemberApprovedEvent(Guid gigId, Guid userId)
        {
            GigId = gigId;
            UserId = userId;
        }
    }
}
