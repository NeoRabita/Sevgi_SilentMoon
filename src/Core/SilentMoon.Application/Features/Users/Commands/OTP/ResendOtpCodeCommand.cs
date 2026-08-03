using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.DTOs.Email;
using SilentMoon.Application.Features.Users.Commands.LoginUser;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Messaging;
using SilentMoon.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Users.Commands.OTP
{
    public partial class ResendOtpCodeCommand : ICommand<RegisterResponse>
    {
        public string OtpId { get; set; }

    }

    public class ResendOtpCodeCommandHandler : ICommandHandler<ResendOtpCodeCommand, RegisterResponse>
    {
        private readonly IAppLogger<LoginUserHandler> _logger;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IOtpService _otpService;
        public ResendOtpCodeCommandHandler(IAppLogger<LoginUserHandler> logger, IMessagePublisher messagePublisher, IOtpService otpService)
        {
            _logger = logger;
            _messagePublisher = messagePublisher;
            _otpService = otpService;
        }
        public async Task<Result<RegisterResponse>> Handle(ResendOtpCodeCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Resend OTP code started for OtpId: {OtpId}", command.OtpId);

            var otp = await _otpService.GetOtpCodeAsync(command.OtpId);
            if (otp.IsFailure)
            {
                _logger.LogInformation("Resend OTP code failed for OtpId: {OtpId}. Error: {Error}", command.OtpId, otp.Error);
                return otp.Error;
            }

            var newOtp = await _otpService.CreateOtpCodeAsync(otp.Value.Email, "Email Verification", "Your verification code is: ");
            await _messagePublisher.PublishAsync(
 "email-queue",
 new EmailRequest
 {
     To = otp.Value.Email,
     Subject = "Email Verification",
     Body = $"Your verification code is: {newOtp.Code}"
 });

            _logger.LogInformation("Resend OTP code successful for OtpId: {OtpId}", command.OtpId);
            return Result<RegisterResponse>.Success(new RegisterResponse
            {
                Message = "Please check your email for verification code.",
                OtpId = newOtp.Id.ToString(),
                OtpExpireAt = newOtp.ExpiresAt.ToShortDateString()
            });
        }
    }//

}
