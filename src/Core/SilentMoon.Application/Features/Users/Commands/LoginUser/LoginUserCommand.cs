using Application.Abstractions.Messaging;
using BCrypt.Net;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SilentMoon.Application.Features.Users.Commands.LoginUser
{
    public partial class LoginUserCommand : ICommand<AuthenticationResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class LoginUserHandler : ICommandHandler<LoginUserCommand, AuthenticationResponse>
    {
        private readonly IUow _uow;
        private readonly IAppLogger<LoginUserHandler> _logger;
        private readonly IJwtService _jwtService;
        private readonly IGenericRepository<ApplicationUser> _genericRepository;
        private readonly IGenericRepository<RefreshToken> _refreshTokensRepository;

        public LoginUserHandler(IUow uow, IAppLogger<LoginUserHandler> logger, IJwtService jwtService, IGenericRepository<ApplicationUser> genericRepository, IGenericRepository<RefreshToken> refreshTokensRepository)
        {
            _uow = uow;
            _logger = logger;
            _jwtService = jwtService;
            _genericRepository = genericRepository;
            _refreshTokensRepository = refreshTokensRepository;
        }

        public async Task<Result<AuthenticationResponse>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User Login started");

            if (command == null)
                return Error.NullValue;
            var existingUser = await _genericRepository.GetAsync(
                  x => x.Email == command.Email);

            if (existingUser is null)
            {
                return UserErrors.NotFoundByEmail;

            }
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(
     command.Password,
     existingUser.PasswordHash);

            if (!isPasswordValid)
            {
                return UserErrors.Unauthorized();
            }

            if (!existingUser.IsEmailConfirmed)
            {
                return UserErrors.EmailNotConfirmed;
            }
            var jwt = _jwtService.GenerateToken(existingUser.Id, existingUser.Email);

            var refreshTokenValue = _jwtService.GenerateRefreshToken();
            var refreshTokenExpires = DateTime.UtcNow.AddDays(7);

            var existingRefreshToken =
                await _refreshTokensRepository.GetAsync(x => x.UserId == existingUser.Id);

            if (existingRefreshToken is null)
            {
                existingRefreshToken = new RefreshToken
                {
                    UserId = existingUser.Id,
                    Token = refreshTokenValue,
                    Expires = refreshTokenExpires,
                    Created = DateTime.UtcNow,
                    CreatedByIp = "127.0.0.1",
                    IsRevoked = false
                };

                await _refreshTokensRepository.AddAsync(existingRefreshToken);
            }
            else
            {
                existingRefreshToken.Token = refreshTokenValue;
                existingRefreshToken.Expires = refreshTokenExpires;
                existingRefreshToken.Created = DateTime.UtcNow;
                existingRefreshToken.IsRevoked = false;

                _refreshTokensRepository.Update(existingRefreshToken);
            }

            await _uow.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {Email} logged in successfully.", existingUser.Email);

            return Result<AuthenticationResponse>.Success(
                new AuthenticationResponse
                {
                    Name = existingUser.FirstName,
                    Email = existingUser.Email,
                    Jwt = jwt,
                    RefreshToken = new RefreshTokenDto(
                        refreshTokenValue,
                        new DateTimeOffset(refreshTokenExpires))
                });






        }
    }


}
