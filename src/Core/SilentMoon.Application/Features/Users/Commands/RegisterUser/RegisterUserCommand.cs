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
        private readonly IAppLogger<RegisterUserCommandHandler> _logger;
        private readonly IOtpService _otpService;

        public RegisterUserCommandHandler(IAuthService authService,  IAppLogger<RegisterUserCommandHandler> logger, IOtpService otpService)
        {
            _authService = authService;
            _logger = logger;
            _otpService = otpService;
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

            var email=await _authService.RegisterAsync(request);

            var otp = await _otpService.CreateAndSendOtpCodeAsync(email.Value, "Email Verification", "Your verification code is: ");

         
            _logger.LogInformation("User successfully created. OtpId: {OtpId}", otp.Id);

            return Result<RegisterResponse>.Success(new RegisterResponse
            {
                Message = "Please check your email for verification code.",
                OtpId = otp.Id.ToString(),
                OtpExpireAt = otp.ExpiresAt.ToShortDateString()

            });


        }
    }

}
