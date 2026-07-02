using SilentMoon.Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task VerifyEmailAsync(string email,string code);
        Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request); 

    }
}
