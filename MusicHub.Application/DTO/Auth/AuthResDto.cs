using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO.Auth
{
    public sealed class AuthResDto
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
}
