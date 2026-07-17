using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.DTOs.JWT;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
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
        private readonly IAuthService _authService;
        private readonly IAppLogger<VerifyEmailCommandHandler> _logger;
        private readonly IUow _uow;
        private readonly IJwtService _jwtService;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
        public VerifyEmailCommandHandler(IAuthService authService, IAppLogger<VerifyEmailCommandHandler> logger, IUow uow, IJwtService jwtService, IGenericRepository<RefreshToken> genericRepository)
        {
            _authService = authService;
            _logger = logger;
            _uow = uow;
            _jwtService = jwtService;
            _refreshTokenRepository = genericRepository;
        }
        public async Task<Result<AuthenticationResponse>> Handle(VerifyEmailCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                return Error.NullValue;
            var result = await _authService.VerifyEmailAsync(command.OtpId,command.Code);
            if (result.IsFailure)
            {
                _logger.LogWarning("OTP verification failed for OtpId: {OtpId}. Error: {Error}", command.OtpId, result.Error);
                return result.Error;
            }

            _logger.LogInformation("OTP verification successful for OtpId: {OtpId}", command.OtpId);
           _logger.LogInformation("User {UserId} has successfully verified their email.", result.Value.Id);


            var jwt = _jwtService.GenerateToken(result.Value.Id, result.Value.Email);


            var refreshToken = new RefreshToken
            {
                Token = _jwtService.GenerateRefreshToken(),
                UserId = result.Value.Id,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = "127.0.0.1",
                IsRevoked = false
            };
            await _refreshTokenRepository.AddAsync(refreshToken);

            await _uow.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("JWT and Refresh Token created for User {UserId}", result.Value.Id);

            return Result<AuthenticationResponse>.Success(new AuthenticationResponse
            {
                Name = result.Value.FirstName,
                Jwt = jwt,
                Email = result.Value.Email,
                RefreshToken = new RefreshTokenDto(refreshToken.Token, new DateTimeOffset(refreshToken.Expires, TimeSpan.Zero))
            });


        }
    }

}
