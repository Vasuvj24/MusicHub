using MusicHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Events
{
    public class EventDispatcher
    {
        public async Task Dispatch(IDomainEvent domainEvent)
        {
            Console.WriteLine($"Event Triggered {domainEvent.GetType().Name}");
            await Task.CompletedTask;
        }
    }
}
