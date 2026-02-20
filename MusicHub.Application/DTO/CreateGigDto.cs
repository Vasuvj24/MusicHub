using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO
{
    public sealed class CreateGigDto
    {
        //for user id we'll get by httpcontext
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime ScheduledAtUtc { get; set; } = DateTime.Now;
    }
}
