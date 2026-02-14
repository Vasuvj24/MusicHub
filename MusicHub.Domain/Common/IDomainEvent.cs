using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Common
{
    public interface IDomainEvent
    {
        DateTime OccuredOn { get; }
    }
}
