using Microsoft.Extensions.Options;
using MusicHub.Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Npgsql.BackendMessages;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Storage
{
    public sealed class CloudinaryStorage: IMediaStorage
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryStorage(IOptions<CloudinarySettings> options)
        {
            var account = new Account(
                options.Value.CloudName,
                options.Value.ApiKey,
                options.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadAsync(Stream fileStream,string fileName,CancellationToken ct)
        {
            var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName,fileStream),Folder = "musichub"
                };

            var result = await _cloudinary.UploadAsync(uploadParams, ct);

            return result.SecureUrl.ToString();
        }

        public async Task<bool> DeleteAsync(string imageUrl,CancellationToken ct)
        {
            try
            {
                var uri = new Uri(imageUrl);

                var publicId = Path.GetFileNameWithoutExtension(uri.AbsolutePath);

                var deleteParams = new DeletionParams($"musichub/{publicId}");

                var result = await _cloudinary.DestroyAsync(deleteParams);

                return result.Result == "ok";
            }
            catch
            {
                return false;
            }
        }
    }
}
