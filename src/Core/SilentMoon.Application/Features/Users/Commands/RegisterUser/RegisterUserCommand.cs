using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Repositories;
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
        private readonly IUow _ouw;

        public RegisterUserCommandHandler(IAuthService authService, IAppLogger<RegisterUserCommandHandler> logger, IOtpService otpService, IUow ouw)
        {
            _authService = authService;
            _logger = logger;
            _otpService = otpService;
            _ouw = ouw;
        }

        public async Task<Result<RegisterResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            
            _logger.LogInformation("User Register started");

            if(command==null)
                    return Error.NullValue;

            var existingUser = await _ouw.UserRepository.GetAsync(
                              x => x.Email == command.Email);

            if (existingUser is not null)
            {
                return UserErrors.EmailNotUnique;
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = command.Name,
                Email = command.Email,
                PasswordHash = passwordHash,
                IsEmailConfirmed = false,
            };

            await _ouw.UserRepository.AddAsync(user);   
            var otp = await _otpService.CreateAndSendOtpCodeAsync(user.Email, "Email Verification", "Your verification code is: ");

         
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
