using MusicHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Events
{
    public sealed class GigCreatedEvent : DomainEvent
    {
        public Guid GigId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public GigCreatedEvent(Guid gigId, Guid createdByUserId)
        {
            GigId = gigId;
            CreatedByUserId = createdByUserId;
        }
    }
}
