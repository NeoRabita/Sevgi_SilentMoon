using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Minio;
using Minio.DataModel.Args;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Enums;
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

        public async Task<Stream> DownloadFileAsync(string fileName,FileType fileType)
        {
            var memoryStream = new MemoryStream();
            var getObjectArgs = new GetObjectArgs()
            .WithBucket(fileType == 0 ? _settings.BucketName : _settings.AudioBucketName)
            .WithObject(fileName)
            .WithCallbackStream((stream) =>
            {
                stream.CopyTo(memoryStream);
            });
            await _minioClient.GetObjectAsync(getObjectArgs);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<string> UploadFileAsync(Stream stream,string fileName,string contentType, FileType fileType)
        {
            var bucketName = fileType == 0 ? _settings.BucketName : _settings.AudioBucketName;
            var bucketExistArgs = new BucketExistsArgs().WithBucket(bucketName);
            bool found = await _minioClient.BucketExistsAsync(bucketExistArgs);
            if (!found)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
                await _minioClient.MakeBucketAsync(makeBucketArgs);
            }
            var extension = Path.GetExtension(fileName);
            var putObjectArgs = new PutObjectArgs()
                               .WithBucket(bucketName)
                               .WithObject(fileName)
                               .WithStreamData(stream)
                               .WithObjectSize(stream.Length)
                               .WithContentType(contentType);
            await _minioClient.PutObjectAsync(putObjectArgs);
            return fileName;
        }


        public string GetUrl(string fileName)
        {
            return $"http://{_settings.Endpoint}/{_settings.BucketName}/{fileName}";
        }
    }
}
