using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SilentMoon.Application.Features.Users.Commands.RegisterUser
{
    public partial class RegisterUserCommand :ICommand<RegisterResponse>
    {
        public string Name { get; set; }    
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }


    }

    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterResponse>
    {
        private readonly IAuthService _authService;
        private readonly IUow _uow;
        private readonly IAppLogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(IAuthService authService, IUow uow, IAppLogger<RegisterUserCommandHandler> logger)
        {
            _authService = authService;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<RegisterResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            
            _logger.LogInformation("User Register started");

            if(command==null)
                    return Error.NullValue;


            var request = new RegisterRequest
            {
                Name = command.Name,
                Email = command.Email,
                Password = command.Password
            };

            var result=await _authService.RegisterAsync(request);
            if (result.IsFailure)
            {
                _logger.LogWarning(
                    "User registration failed for email {Email}. Error: {Error}",
                    command.Email,
                    result.Error);

                return result.Error;
            }

            await _uow.SaveChangesAsync();

            _logger.LogInformation("User successfully created. OtpId: {OtpId}", result.Value.OtpId);

            return result;

        }
    }

}
