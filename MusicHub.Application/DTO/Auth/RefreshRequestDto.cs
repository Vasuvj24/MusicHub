using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO.Auth
{
    public sealed class RefreshRequestDto
    {
        public string RefreshToken { get; set; } = "";
    }
}
