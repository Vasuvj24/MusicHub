using MusicHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Time
{
    public sealed class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
