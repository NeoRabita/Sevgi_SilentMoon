using SilentMoon.Application.DTOs.Account;
using SilentMoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> CreateApplicationUserAsync(RegisterRequest registerRequest);
        Task<bool> UserExistsAsync(string email);

    }
}
