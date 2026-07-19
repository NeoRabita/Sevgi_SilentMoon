using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilentMoon.Application.Features.Users.Commands.GoogleAuthorize;
using SilentMoon.Application.Features.Users.Queries.Profile;
using System.Threading.Tasks;

namespace SilentMoon.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : BaseController
    {
        [HttpGet("me")]
        public async Task<IResult> GoogleLogin()
        {
            var result = await Dispatcher.Send(new GetUserProfileQuery());
            return HandleResult(result);

        }

    }
}
