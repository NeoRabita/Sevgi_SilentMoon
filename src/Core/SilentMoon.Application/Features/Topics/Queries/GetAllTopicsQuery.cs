using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Topic;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Topics.Queries
{
    public  class GetAllTopicsQuery:IQuery<IEnumerable<TopicResponse>>
    {
    }

    public class GetAllTopicsQueryHandler : IQueryHandler<GetAllTopicsQuery, IEnumerable<TopicResponse>>
    {
        private readonly IUow _uow;
        private readonly IAppLogger<GetAllTopicsQuery> _logger;

        public GetAllTopicsQueryHandler(IUow uow, IAppLogger<GetAllTopicsQuery> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<TopicResponse>>> Handle(GetAllTopicsQuery query, CancellationToken ct)
        {
            _logger.LogInformation("Get all topics process started");
            var list = await _uow.TopicRepository.GetAllTopicsAsync();

            _logger.LogInformation("Get all topics process ended");
            return Result<IEnumerable<Topic>>.Success(list);
        }
    }
}
