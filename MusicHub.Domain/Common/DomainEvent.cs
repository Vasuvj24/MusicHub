using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Common
{
    //we inherit Domain event when we need to make it event and when we need add some events then we need to inherit baseentity
    public class DomainEvent : IDomainEvent
    {
        //creation of domainevent timelines
        public DateTime OccuredOn { get; protected set; } = DateTime.UtcNow;
    }
}
