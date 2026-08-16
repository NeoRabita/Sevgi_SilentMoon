using SilentMoon.Domain.Enums;
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
        public Task<string> UploadFileAsync(Stream stream, string fileName,string contentType, FileType fileType);
        public Task<Stream> DownloadFileAsync(string fileName, FileType fileType);
        public string GetUrl(string fileName);
    }
}
