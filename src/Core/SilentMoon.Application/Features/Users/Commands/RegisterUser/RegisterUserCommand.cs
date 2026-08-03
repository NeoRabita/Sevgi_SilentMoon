using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.DTOs.Email;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Messaging;
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
        private readonly IAppLogger<RegisterUserCommandHandler> _logger;
        private readonly IOtpService _otpService;
        private readonly IUow _ouw;
        private readonly IMessagePublisher _messagePublisher;

        public RegisterUserCommandHandler(IAppLogger<RegisterUserCommandHandler> logger, IOtpService otpService, IUow ouw, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _otpService = otpService;
            _ouw = ouw;
            _messagePublisher = messagePublisher;
        }

        public async Task<Result<RegisterResponse>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            
            _logger.LogInformation("User Register started");

            

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
            var otp = await _otpService.CreateOtpCodeAsync(user.Email, "Email Verification", "Your verification code is: ");
            await _messagePublisher.PublishAsync(
    "email-queue",
    new EmailRequest
    {
        To = user.Email,
        Subject = "Email Verification",
        Body = $"Your verification code is: {otp.Code}"
    });

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
