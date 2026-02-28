using MusicHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO.Admin
{
    public sealed class ReportPostDto
    {
        public ReportReason Reason { get; set; }
        public string? Note { get; set; }
    }
}
