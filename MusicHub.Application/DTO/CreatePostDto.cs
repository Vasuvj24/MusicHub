using MusicHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO
{
    //Dto is data to transfer
    public sealed class CreatePostDto
    {
        //public Guid UserId { get; set; }
        public InstrumentType Instrument { get; set; }
        public string MediaUrl { get; set; } = "";
        //add null reference since post can have no caption
        public string? Caption { get; set; }
    }
}
