using MusicHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Entities
{
    public class GigMember
    {
        public Guid GigId { get; private set; }
        public Guid UserId { get; private set; }
        public InstrumentType Instrument { get; private set; }
        public GigMemberStatus Status { get; private set; } = GigMemberStatus.Pending;
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        public GigMember(Guid userId, InstrumentType instrument)
        {
            //GigId = gigId;
            UserId = userId;
            Instrument = instrument;
            Status = GigMemberStatus.Pending;
        }
        public void Approve() => Status = GigMemberStatus.Approved;
        public void Reject() => Status = GigMemberStatus.Rejected;
    }
}
