using Microsoft.Extensions.Options;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Infrastructure.Persistence.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Persistence.Services
{
    public class StorageService : IStorageService
    {
        private readonly APIAppSettings _settings;

        public StorageService(IOptions<APIAppSettings> settings)
        {
            _settings = settings.Value;
        }

        public Task DownloadFileAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetFileAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetUrl()
        {
            throw new NotImplementedException();
        }

        public Task UpdateFileAsync(string file)
        {
            throw new NotImplementedException();
        }
    }
}
