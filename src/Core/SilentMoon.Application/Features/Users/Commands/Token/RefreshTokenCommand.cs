using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.DTOs.JWT;
using SilentMoon.Application.Features.Users.Commands.LoginUser;
using SilentMoon.Application.Interfaces.Logging;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SilentMoon.Application.Features.Users.Commands.Token
{
    public partial class RefreshTokenCommand:ICommand<AuthenticationResponse>
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthenticationResponse>
    {
        private readonly IUow _uow;
        private readonly IAppLogger<LoginUserHandler> _logger;
        private readonly IJwtService _jwtService;
        public RefreshTokenCommandHandler( IUow uow, IAppLogger<LoginUserHandler> logger, IJwtService jwtService)
        {
            _uow = uow;
            _logger = logger;
            _jwtService = jwtService;
        }
        public async Task<Result<AuthenticationResponse>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Refresh token process started.");
           
            var token = await _uow.RefreshTokenRepository.GetAsync(r => r.Token == command.RefreshToken);
            if (token is null) { return UserErrors.Unauthorized(); }
            if (!token.IsActive)
            {
                return UserErrors.Unauthorized();
            }

            var user = await _uow.UserRepository.GetAsync(
            x => x.Id == token.UserId);

            if (user is null)
                return UserErrors.NotFoundByEmail;
            var jwt = _jwtService.GenerateToken(user.Id, user.Email);

            _uow.RefreshTokenRepository.Update(_jwtService.UpdateRefreshToken(token));

            _logger.LogInformation("Refresh token process completed.");
            return Result<AuthenticationResponse>.Success(
                new AuthenticationResponse
                {
                    Name = user.FirstName,
                    Email = user.Email,
                    Jwt = jwt,
                    RefreshToken = new RefreshTokenDto(
                        token.Token,
                        new DateTimeOffset(token.Expires, TimeSpan.Zero))
                });

           


        }
    }

}
