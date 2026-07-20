using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.DTOs.JWT;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
using SilentMoon.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SilentMoon.Application.Features.Users.Commands.OTP
{
    public partial class VerifyEmailCommand:ICommand<AuthenticationResponse>
    {
        public string OtpId { get; set; }
        public string Code { get; set; }
    }

    public class VerifyEmailCommandHandler :ICommandHandler<VerifyEmailCommand, AuthenticationResponse>
    {
        private readonly IAppLogger<VerifyEmailCommandHandler> _logger;
        private readonly IJwtService _jwtService;
        private readonly IUow _uow;
        private readonly IOtpService _otpService;
        public VerifyEmailCommandHandler( IAppLogger<VerifyEmailCommandHandler> logger, IJwtService jwtService,IUow uow, IOtpService otpService)
        {
            _logger = logger;
            _jwtService = jwtService;
            _uow = uow;
            _otpService = otpService;
        }
        public async Task<Result<AuthenticationResponse>> Handle(VerifyEmailCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return Error.NullValue;

            var otp = await _otpService.VerifyOtpCodeAsync(command.OtpId, command.Code);
            if (otp.IsFailure)
            {
                return OtpErrors.InvalidCode;
            }

            var user = await _uow.UserRepository.GetAsync(
                    x => x.Email == otp.Value.Email);
            if (user == null)
            {
                _logger.LogWarning("OTP verification failed for OtpId: User not found: {Email}", otp.Value.Email);
                return UserErrors.NotFoundByEmail;
            }
            if (!user.IsEmailConfirmed)
            {
                user.IsEmailConfirmed = true;
                _uow.UserRepository.Update(user);
            }


            _logger.LogInformation("OTP verification successful for OtpId: {OtpId}", command.OtpId);
           _logger.LogInformation("User {UserId} has successfully verified their email.", user.Id);


            var jwt = _jwtService.GenerateToken(user.Id, user.Email);


            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            await _uow.RefreshTokenRepository.AddAsync(refreshToken);

            _logger.LogInformation("JWT and Refresh Token created for User {UserId}", user.Id);

            return Result<AuthenticationResponse>.Success(new AuthenticationResponse
            {
                Name = user.FirstName,
                Jwt = jwt,
                Email = user.Email,
                RefreshToken = new RefreshTokenDto(refreshToken.Token, new DateTimeOffset(refreshToken.Expires, TimeSpan.Zero))
            });


        }
    }

}
