using Application.Abstractions.Messaging;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
using SilentMoon.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Courses.Commands
{
    public class CreateCourseCommand:ICommand<bool>
    {
        public Stream stream   { get; set; }
        public string Filename { get; set; }
        public string Subtitle { get; set; }
        public string Type { get; set; }
        public int CategoryId { get; set; }
        public int DurationSec { get; set; }
        public bool IsFeatured { get; set; }
        public NarratorType NarratorType { get; set; }

        public string Title_AZ { get; set; }
        public string Title_RU { get; set; }
        public string Title_EN { get; set; }



    }


    public class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand,bool>
    {
        private readonly IStorageService _storageService;
        public readonly IUow _uow;

        public CreateCourseCommandHandler(IStorageService storageService, IUow uow)
        {
            _storageService = storageService;
            _uow = uow;
        }

        public async Task<Result<bool>> Handle(CreateCourseCommand command, CancellationToken ct)
        {
           var url= await _storageService.UploadFileAsync(command.stream, command.Filename, "jpg", FileType.Image);
            var translationId ="C_" + Guid.NewGuid();
            var translation = new Translation
            {
                Id =translationId ,
                Translation_AZ = command.Title_AZ,
                Translation_RU = command.Title_RU,
                Translation_EN = command.Title_EN,
            };

            await _uow.TranslationRepository.AddAsync(translation);

            var course = new Course
            {
                Title = command.Title_EN,
                CategoryId = command.CategoryId,
                DurationSec = command.DurationSec,
                IsFeatured = command.IsFeatured,
                NarratorType = command.NarratorType,
                ImageUrl = url,
                Subtitle = command.Subtitle,
                TranslationId=translationId,
            };
            await _uow.CourseRepository.AddAsync(course);

            return true;
        }
    }
}
