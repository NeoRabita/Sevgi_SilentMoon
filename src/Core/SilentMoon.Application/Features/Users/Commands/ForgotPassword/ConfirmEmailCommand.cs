using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.Interfaces.Logging;
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

        public ForgotPasswordCommandHandler( IAppLogger<ConfirmEmailCommand> logger, IOtpService otpService)
        {
            _logger = logger;
            _otpService = otpService;
        }

        public async Task<Result<RegisterResponse>> Handle(ConfirmEmailCommand command, CancellationToken ct)
        {
          var otp= await _otpService.CreateAndSendOtpCodeAsync(command.Email, "Email Verification for Forgot Password", "Your verification code is: ");

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
