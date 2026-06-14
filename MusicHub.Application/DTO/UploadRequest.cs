using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.DTO
{
    public sealed class UploadRequest
    {
        public IFormFile File { get; set; } = default!;
    }
}
