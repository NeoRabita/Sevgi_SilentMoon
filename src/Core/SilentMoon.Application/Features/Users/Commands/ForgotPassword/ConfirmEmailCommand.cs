using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.DTOs.Email;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Messaging;
using SilentMoon.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SilentMoon.Application.Features.Users.Commands.ForgotPassword
{
    public class ConfirmEmailCommand:ICommand<RegisterResponse>
    {
        public string Email{ get; set; }

    }

    public class ForgotPasswordCommandHandler : ICommandHandler<ConfirmEmailCommand, RegisterResponse>
    {
        private readonly IAppLogger<ConfirmEmailCommand> _logger;
        private readonly IOtpService _otpService;
        private readonly IMessagePublisher _messagePublisher;
        public ForgotPasswordCommandHandler(IAppLogger<ConfirmEmailCommand> logger, IOtpService otpService, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _otpService = otpService;
            _messagePublisher = messagePublisher;
        }

        public async Task<Result<RegisterResponse>> Handle(ConfirmEmailCommand command, CancellationToken ct)
        {
          var otp= await _otpService.CreateOtpCodeAsync(command.Email, "Email Verification for Forgot Password", "Your verification code is: ");
            await _messagePublisher.PublishAsync(
    "email-queue",
    new EmailRequest
    {
        To = command.Email,
        Subject = "Email Verification for Forgot Password",
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
