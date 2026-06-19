using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
    public interface IMediaStorage
    {
        Task<string> UploadAsync(
            Stream fileStream,
            string fileName,
            CancellationToken ct);

        Task<bool> DeleteAsync(
            string publicId,
            CancellationToken ct);
    }
}
