using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Topic;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
using SilentMoon.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Topics.Queries
{
    public class GetSelectedTopicsQuery:IQuery<IEnumerable<TopicResponse>>
    {
    }

    public class GetSelectedTopicsQueryHandler : IQueryHandler<GetSelectedTopicsQuery, IEnumerable<TopicResponse>>
    {
        private readonly IUow _uow;
        private readonly IAppLogger<GetSelectedTopicsQueryHandler> _logger;
        private readonly IUserService _userService;

        public GetSelectedTopicsQueryHandler(IUow uow, IAppLogger<GetSelectedTopicsQueryHandler> logger, IUserService userService)
        {
            _uow = uow;
            _logger = logger;
            _userService = userService;
        }

        public async Task<Result<IEnumerable<TopicResponse>>> Handle(GetSelectedTopicsQuery query, CancellationToken ct)
        {
         var user=await _userService.GetCurrentUserAsync();
            if (user.IsFailure) {
                return UserErrors.Unauthorized();
            }

            var userTopicList =await _uow.UserTopicRepository.GetAllAsync(ut=>ut.UserId==user.Value.Id,ct);
            var topicIds = userTopicList
    .Select(x => x.TopicId)
    .ToList();

            var topics = await _uow.TopicRepository.GetAllAsync(
    t => topicIds.Contains(t.Id),
    ct);
            var response = topics.Select(x => new TopicResponse
            {
                Id = x.Id,
                Slug = x.Slug,
                Title = x.Title,
                IconKey = x.IconKey,
                ColorHex = x.ColorHex
            });

            return Result<IEnumerable<Topic>>.Success(response);

        }
    }
}
