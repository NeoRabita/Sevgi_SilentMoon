using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Profile;
using SilentMoon.Application.Features.Users.Commands.RegisterUser;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Users.Commands.Profile
{
    public class UpdateProfileCommand : ICommand<UpdateUserProfileResponse>
    {
        public string Name { get; set; }
    }


    public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, UpdateUserProfileResponse>
    {
        private readonly IProfileService _profileService;
        private readonly IAppLogger<RegisterUserCommandHandler> _logger;


        public UpdateProfileCommandHandler(IProfileService profileService, IAppLogger<RegisterUserCommandHandler> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        public async Task<Result<UpdateUserProfileResponse>> Handle(UpdateProfileCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Update profile process started");
            var result = await _profileService.UpdateUserProfileAsync(command.Name);
            if (result.IsFailure)
            {
                _logger.LogInformation("Update profile process failed");
                return result.Error;
            }
                _logger.LogInformation("Update profile process succeeded. ");

            return result;
        }
    }

}
