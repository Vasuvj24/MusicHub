using Microsoft.AspNetCore.Hosting;
using MusicHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Storage
{
    public class LocalMediaStorage : IMediaLocalStorage
    {
        private readonly string _root;
        public LocalMediaStorage(IWebHostEnvironment env)
        {
            _root = Path.Combine(env.ContentRootPath, "uploads");
            Directory.CreateDirectory(_root);
        }
        public async Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken cts)
        {
            //using path.getfilename to get the directory traversal attack
            var safeName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
            var fullPath = Path.Combine(_root, safeName);

            await using var fs = File.Create(fullPath);
            await fileStream.CopyToAsync(fs, cts);

            // later replace with CDN/S3
            return $"/uploads/{safeName}";
        }
        public async Task<bool> DeleteAsync(string fileName, CancellationToken ct)
        {
            var path = Path.Combine(_root, fileName);

            if (!File.Exists(path))
                return false;

            File.Delete(path);
            return true;
        }
    }
}
