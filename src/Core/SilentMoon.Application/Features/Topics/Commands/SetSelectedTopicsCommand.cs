using Application.Abstractions.Messaging;
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
using System.Windows.Input;

namespace SilentMoon.Application.Features.Topics.Commands
{
    public class SetSelectedTopicsCommand:ICommand<bool>
    {
        public IEnumerable<int> TopicIds { get; set; }
    }

    public class SetSelectedTopicsCommandHandler : ICommandHandler<SetSelectedTopicsCommand, bool>
    {
        private readonly IUow _uow;
        private readonly IAppLogger<SetSelectedTopicsCommandHandler> _logger;
        private readonly IUserService _userService;

        public SetSelectedTopicsCommandHandler(IUow uow, IAppLogger<SetSelectedTopicsCommandHandler> logger, IUserService userService)
        {
            _uow = uow;
            _logger = logger;
            _userService = userService;
        }

        public async Task<Result<bool>> Handle(SetSelectedTopicsCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Set selected topics started");
            var user =await _userService.GetCurrentUserAsync();
            if (user.IsFailure) { 
                
            _logger.LogInformation("Set selected topics :User not authorized");
                return UserErrors.Unauthorized(); }
            var userTopics = command.TopicIds
                  .Select(topicId => new UserTopic
                  {
                      UserId = user.Value.Id,
                      TopicId = topicId
                  });

            await _uow.UserTopicRepository.AddRangeAsync(userTopics, ct);

            _logger.LogInformation("Set selected topics succeeded");
            return true;


        }
    }
}
