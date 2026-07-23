using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilentMoon.Application.Features.Topics.Commands;
using SilentMoon.Application.Features.Topics.Queries;
using SilentMoon.Application.Features.Users.Commands.ForgotPassword;
using System.Threading.Tasks;

namespace SilentMoon.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnboardingController : BaseController
    {
        
        [HttpGet("topics")]
        public async Task<IResult> Topics(  )
        {
            var result = await Dispatcher.Send(new GetAllTopicsQuery());
            return HandleResult(result);

        }
        
        [HttpGet("me/topics")]
        public async Task<IResult> SelectedTopics(  )
        {
            var result = await Dispatcher.Send(new GetSelectedTopicsQuery());
            return HandleResult(result);

        }

        [HttpPut("me/topics")]
        public async Task<IResult> SetTopics([FromBody] SetSelectedTopicsCommand command)
        {
            var result = await Dispatcher.Send(command);
            return HandleResult(result);

        }
    }
}
