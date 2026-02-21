using MusicHub.Domain.Common;
using MusicHub.Domain.Enums;
using MusicHub.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Entities
{
    public class Gig : BaseEntity
    {
        //backing field for Members so that can't be modified by outside world
        //these backing field can't be removed or tampered with only few operations are allowed on them => encapsulation +domainan safety
        private readonly List<GigMember> _members = new();
        public Guid CreatedByUserId { get; private set; }
        public string Title { get; private set; } = "";
        public string Description { get; private set; } = "";
        public DateTime ScheduledAtUtc { get; private set; }
        public GigStatus Status { get; private set; } = GigStatus.Open;

        public IReadOnlyCollection<GigMember> Members => _members;
        private Gig() { } // EF
        public Gig(Guid createdByUserId, string title, string description, DateTime scheduledAtUtc)
        {
            if (createdByUserId == Guid.Empty) throw new ArgumentException("Invalid creator");
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title required");
            if (scheduledAtUtc <= DateTime.UtcNow) throw new ArgumentException("Gig must be in future");
            CreatedByUserId = createdByUserId;
            Title = title.Trim();
            Description = description?.Trim() ?? "";
            ScheduledAtUtc = scheduledAtUtc;
            Status = GigStatus.Open;

            AddDomainEvent(new GigCreatedEvent(Id, CreatedByUserId));
        }
        public void Apply(Guid userId,InstrumentType instrument)
        {
            if (Status != GigStatus.Open) throw new InvalidOperationException("Gig is closed");
            if (_members.Any(m => m.UserId == userId))
                throw new InvalidOperationException("Already applied/joined");

            _members.Add(new GigMember(userId, instrument));
            AddDomainEvent(new GigAppliedEvent(Id, userId));
        }
        public void ApproveMember(Guid actorId, Guid memberUserId)
        {
            if (actorId != CreatedByUserId)
                throw new UnauthorizedAccessException("only gig creators can approve");
            var member = _members.SingleOrDefault(m => m.UserId == memberUserId) ?? throw new KeyNotFoundException("member not found");
            member.Approve();   
            AddDomainEvent(new GigMemberApprovedEvent(Id, memberUserId));
        }
        public void Close(Guid actorUserId)
        {
            if (actorUserId != CreatedByUserId)
                throw new UnauthorizedAccessException("Only gig creator can close");
            Status = GigStatus.Closed;
        }
    }
}
