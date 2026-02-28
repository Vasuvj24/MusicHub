using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
