using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(Stream file, string fileName, string contentType);

        Task UpdateFileAsync(Stream file, string fileName, string contentType);

        Task<Stream> GetFileAsync(string fileName);

        Task<byte[]> DownloadFileAsync(string fileName);

        Task<string> GetUrlAsync(string fileName);

        Task DeleteFileAsync(string fileName);
    }
}
