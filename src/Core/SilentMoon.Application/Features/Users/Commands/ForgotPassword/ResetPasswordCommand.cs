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

namespace SilentMoon.Application.Features.Users.Commands.ForgotPassword
{
    public  class ResetPasswordCommand:ICommand<ForgotPasswordResponse>
    {
        public string OtpId { get; set; }
        public string OtpCode { get; set; }
        public string Password { get; set; }


    }

    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, ForgotPasswordResponse>
    {
        private readonly IUow _uow;
        private readonly IAppLogger<ResetPasswordCommand> _logger;
        private readonly IOtpService _otpService;

        public ResetPasswordCommandHandler(IUow uow, IAppLogger<ResetPasswordCommand> logger, IOtpService otpService)
        {
            _uow = uow;
            _logger = logger;
            _otpService = otpService;
        }

        public async Task<Result<ForgotPasswordResponse>> Handle(ResetPasswordCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Reset password process started");
            var result =await _otpService.VerifyOtpCodeAsync(command.OtpId, command.OtpCode);
            if (result.IsFailure)
            {
                _logger.LogInformation("Otp verification failed");
                return result.Error;
            }
            _logger.LogInformation("Otp code verified. User Email: {Email}",result.Value.Email);
            var user = await _uow.UserRepository.GetAsync(u => u.Email == result.Value.Email);
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);
            user.PasswordHash = passwordHash;
            _uow.UserRepository.Update(user);
            _logger.LogInformation("User password reseted. User Email: {Email}",result.Value.Email);
            return new ForgotPasswordResponse { Message = "User passwrod reseted successfully." };



        }
    }
}
