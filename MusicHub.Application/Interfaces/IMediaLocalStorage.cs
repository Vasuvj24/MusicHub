using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
    public interface IMediaLocalStorage
    {
        Task<bool> DeleteAsync(string fileName, CancellationToken cts);
        Task<string> SaveAsync(Stream fileStream, string fileName, CancellationToken cts);
    }
}
