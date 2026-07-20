using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Profile;
using SilentMoon.Application.Features.Users.Commands.RegisterUser;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SilentMoon.Application.Features.Users.Commands.Profile
{
    public class UpdateProfileCommand : ICommand<UpdateUserProfileResponse>
    {
        public string Name { get; set; }
    }


    public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, UpdateUserProfileResponse>
    {
        private readonly IAppLogger<RegisterUserCommandHandler> _logger;
        private readonly IUserService _userService;
        private readonly IUow _uow;

        public UpdateProfileCommandHandler(IAppLogger<RegisterUserCommandHandler> logger, IUserService userService, IUow uow)
        {
            _logger = logger;
            _userService = userService;
            _uow = uow;
        }

        public async Task<Result<UpdateUserProfileResponse>> Handle(UpdateProfileCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Update profile process started");
            var userResult = await _userService.GetCurrentUserAsync();

            if (userResult.IsFailure)
            {
                _logger.LogInformation("Update profile process failed");

                return userResult.Error;
            }
            var user = userResult.Value;
            user.FirstName = command.Name;
           _uow.UserRepository.Update(user);
            _logger.LogInformation("Update profile process succeeded. ");

            return Result<UpdateUserProfileResponse>.Success(
            new UpdateUserProfileResponse
            {
                Name = user.FirstName
            });



        }
    }

}
