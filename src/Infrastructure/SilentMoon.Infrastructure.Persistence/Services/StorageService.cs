using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Infrastructure.Persistence.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Persistence.Services
{
    public class StorageService : IStorageService
    {
        private readonly MinioSettings _settings;
        private readonly IMinioClient _minioClient;

        public StorageService(IOptions<APIAppSettings> settings, IMinioClient minioClient)
        {
            _settings = settings.Value.MinioSettings;
            _minioClient = minioClient;
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var memoryStream = new MemoryStream();
            var getObjectArgs = new GetObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(fileName)
            .WithCallbackStream((stream) =>
            {
                stream.CopyTo(memoryStream);
            });
            await _minioClient.GetObjectAsync(getObjectArgs);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var bucketExistArgs = new BucketExistsArgs().WithBucket(_settings.BucketName);
            bool found = await _minioClient.BucketExistsAsync(bucketExistArgs);
            if (!found)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(_settings.BucketName);
                await _minioClient.MakeBucketAsync(makeBucketArgs);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            using var stream = file.OpenReadStream();
            var putObjectArgs = new PutObjectArgs()
       .WithBucket(_settings.BucketName)
       .WithObject(fileName)
       .WithStreamData(stream)
       .WithObjectSize(file.Length)
       .WithContentType(file.ContentType);
            await _minioClient.PutObjectAsync(putObjectArgs);
            return fileName;
        }


        public string GetUrl(string fileName)
        {
            return $"http://{_settings.Endpoint}/{_settings.BucketName}/{fileName}";
        }
    }
}
