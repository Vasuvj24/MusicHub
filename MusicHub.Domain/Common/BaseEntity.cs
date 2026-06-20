using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Common
{
    //making abstract as needed some function of my own implementation and some needs implementation
    //used this to add event like post like and other event so that can incorporate logs or notification
    //using domain driven architecture
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAtUtc { get; protected set; }

        public DateTime? UpdatedAtUtc { get; protected set; }

        //this is a property that stores the actual data
        private readonly List<IDomainEvent> _domainEvents = new();
        //this is field that helps to access data in controlled way to others 
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent); 
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();  
        }
        public void MarkUpdated()
        {
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
