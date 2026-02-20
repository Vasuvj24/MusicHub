using MusicHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Events
{
    public sealed class GigAppliedEvent : DomainEvent
    {
        public Guid GigId { get; }
        public Guid UserId { get; }

        public GigAppliedEvent(Guid gigId, Guid userId)
        {
            GigId = gigId;
            UserId = userId;
        }
    }
}
