using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Common
{
    public class DomainEvent : IDomainEvent
    {
        //creation of domainevent timelines
        public DateTime OccuredOn { get; protected set; } = DateTime.UtcNow;
    }
}
