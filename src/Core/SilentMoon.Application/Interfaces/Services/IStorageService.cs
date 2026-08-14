using Microsoft.AspNetCore.Http;
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
        public Task<string> UploadFileAsync(IFormFile file);
        public Task<Stream> DownloadFileAsync(string fileName);
        public string GetUrl(string fileName);
    }
}
