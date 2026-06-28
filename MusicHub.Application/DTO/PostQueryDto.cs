using MusicHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO
{
    public class PostQueryDto
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public InstrumentType? Instrument { get; set; }

        public string SortBy { get; set; } = "date";
    }
}
