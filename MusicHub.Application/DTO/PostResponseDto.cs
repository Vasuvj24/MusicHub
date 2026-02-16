using MusicHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace MusicHub.Application.DTO
{
    public sealed class PostResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public InstrumentType Instrument {  get; set; }
        public string MediaUrl { get; set; }
        public string Caption { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
