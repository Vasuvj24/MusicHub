using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO.Auth
{
    public sealed class MeDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";    
    }
}
