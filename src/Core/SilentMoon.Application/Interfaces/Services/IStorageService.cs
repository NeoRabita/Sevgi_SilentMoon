using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Services
{
    public interface IStorageService
    {
        Task UpdateFileAsync(string file);
        Task GetFileAsync();
        Task DownloadFileAsync();
        Task GetUrl();
    }
}
