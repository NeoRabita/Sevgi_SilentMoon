using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Users.Commands.GoogleAuthorize
{
    public partial class GoogleLoginCommand : ICommand<AuthenticationResponse>
    {
        public string IdToken { get; set; }
    }

    public class GoogleLoginCommandHandler
        : ICommandHandler<GoogleLoginCommand, AuthenticationResponse>
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IAppLogger<GoogleLoginCommandHandler> _logger;

        public GoogleLoginCommandHandler(
            IAuthService authService,
            IJwtService jwtService,
            IAppLogger<GoogleLoginCommandHandler> logger)
        {
            _authService = authService;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<Result<AuthenticationResponse>> Handle(
            GoogleLoginCommand command,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Google login process started.");
            var user = await _authService.GoogleLoginAsync(command.IdToken);
            if (user.IsFailure)
            {
                _logger.LogWarning("Google authentication failed.");
                return user.Error;
            }

            var jwt = _jwtService.GenerateToken(user.Value.Id, user.Value.Email);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Value.Id);
            _logger.LogInformation(
    "Google authentication succeeded for {Email}.",
    user.Value.Email);
            return Result<AuthenticationResponse>.Success(new AuthenticationResponse
            {
                Name = user.Value.FirstName,
                Jwt = jwt,
                Email = user.Value.Email,
                RefreshToken = new DTOs.JWT.RefreshTokenDto
                (
                     refreshToken.Token,
                    refreshToken.Expires
                )
            });


        }
    }
}