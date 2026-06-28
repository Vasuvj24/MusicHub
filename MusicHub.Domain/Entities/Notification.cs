using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; private set; } = false;

        public DateTime CreatedAtUtc { get; set; }
        public void MarkRead()
        {
            IsRead = true;
        }
        public void MarkUnRead()
        {
            IsRead = false;
        }
    }
}
