using Microsoft.AspNetCore.Http;
using SilentMoon.Application.Common.User;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SilentMoon.WebApi.Middlewares
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICurrentUser currentUser)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                

                currentUser.Email =
                    context.User.FindFirst(ClaimTypes.Email)?.Value;
            }

            await _next(context);
        }
    }
}
