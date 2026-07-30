using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.Features.Users.Commands.LoginUser;
using SilentMoon.Application.Interfaces.Logging;
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
        private readonly IAuthService _authService;
        private readonly IAppLogger<LoginUserHandler> _logger;
        public ResendOtpCodeCommandHandler(IAuthService authService, IAppLogger<LoginUserHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        public async Task<Result<RegisterResponse>> Handle(ResendOtpCodeCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Resend OTP code started for OtpId: {OtpId}", command.OtpId);
           
            var result = await _authService.ResendOtp(command.OtpId);
            if (result.IsFailure)
            {
                _logger.LogInformation("Resend OTP code failed for OtpId: {OtpId}. Error: {Error}", command.OtpId, result.Error);
                return Result.Failure<RegisterResponse>(result.Error);
            }
            _logger.LogInformation("Resend OTP code successful for OtpId: {OtpId}", command.OtpId);
            return Result.Success(result.Value);
        }
    }

}
