using MusicHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Events
{
    public class UserRegisteredEvent : DomainEvent
    {
        public Guid UserId { get; }
        public UserRegisteredEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
