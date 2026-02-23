using MusicHub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO.Profile
{
    //adding a service of a user
    public sealed class AddServiceDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
    }
}
