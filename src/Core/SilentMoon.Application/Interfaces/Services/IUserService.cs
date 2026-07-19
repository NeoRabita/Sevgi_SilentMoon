using SilentMoon.Domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<ApplicationUser>> GetCurrentUserAsync();


    }
}