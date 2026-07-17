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
using System.Windows.Input;

namespace SilentMoon.Application.Features.Users.Commands.Token
{
    public partial class RefreshTokenCommand:ICommand<AuthenticationResponse>
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthenticationResponse>
    {
        private readonly IAuthService _authService;
        private readonly IUow _uow;
        private readonly IAppLogger<LoginUserHandler> _logger;
        public RefreshTokenCommandHandler(IAuthService authService, IUow uow, IAppLogger<LoginUserHandler> logger)
        {
            _authService = authService;
            _uow = uow;
            _logger = logger;
        }
        public async Task<Result<AuthenticationResponse>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Refresh token process started.");
            if (command == null)
                return Error.NullValue;
            var result = await _authService.RefreshTokenAsync(command.RefreshToken);
            await _uow.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Refresh token process completed.");
            return result;
        }
    }

}
