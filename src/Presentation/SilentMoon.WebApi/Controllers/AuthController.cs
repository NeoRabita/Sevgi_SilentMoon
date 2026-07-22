using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Application.Features.Users.Commands.RegisterUser;
using System.Threading.Tasks;
using SilentMoon.Application.Features.Users.Commands.OTP;
using SilentMoon.Application.Features.Users.Commands.LoginUser;
using SilentMoon.Application.Features.Users.Commands.Token;
using SilentMoon.Application.Features.Users.Commands.GoogleAuthorize;
using SilentMoon.Application.Features.Users.Commands.ForgotPassword;

namespace SilentMoon.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {

        [HttpPost("register")]
        public async Task<IResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await Dispatcher.Send(command);
            return HandleResult(result);

        }
        
        [HttpPost("verify-email")]
        public async Task<IResult> VerifyEmail([FromBody] VerifyEmailCommand command)
        {
            var result = await Dispatcher.Send(command);
            return HandleResult(result);

        }

        [HttpPost("login")]
        public async Task<IResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await Dispatcher.Send(command);
            return HandleResult(result);

        }


        [HttpPost("refresh")]
        public async Task<IResult> Refresh([FromBody] RefreshTokenCommand command)
        {
            var result = await Dispatcher.Send(command);
            return HandleResult(result);

        }
        
        [HttpPost("resend-otp")]
        public async Task<IResult> ResendOtp([FromBody] ResendOtpCodeCommand command)
        {
            var result = await Dispatcher.Send(command);
            return HandleResult(result);

        }
         
        [HttpPost("oauth/google")]
        public async Task<IResult> GoogleLogin([FromBody] GoogleLoginCommand command)
        {
            var result = await Dispatcher.Send(command);
            return HandleResult(result);

        }


        [HttpPost("confirm-email")]
        public async Task<IResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
        {
            var result = await Dispatcher.Send(command);
            return HandleResult(result);

        }

        [HttpPatch("forgot-password")]
        public async Task<IResult> ForgotPassword([FromBody]ResetPasswordCommand command)
        {
            var result = await Dispatcher.Send(command);
            return HandleResult(result);

        }






    }
}
