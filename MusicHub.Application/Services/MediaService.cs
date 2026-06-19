using Microsoft.AspNetCore.Http;
using MusicHub.Application.DTO.Media;
using MusicHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Services
{
    public sealed class MediaService
    {
        private readonly IMediaStorage _storage;
        public MediaService(IMediaStorage storage)
        {
            _storage = storage;
        }
        public async Task<UploadResultDto> UploadAsync(IFormFile file,CancellationToken cts)
        {
            if (file.Length == 0)
                throw new ArgumentException("Empty file");

            await using var stream =file.OpenReadStream();

            var url = await _storage.UploadAsync(stream,file.FileName,cts);

            return new UploadResultDto
            {
                Url = url
            };
        }
        public async Task<bool> DeleteAsync(string fileName, CancellationToken cts)
        {
            return await _storage.DeleteAsync(fileName, cts);
        }
    }
}
