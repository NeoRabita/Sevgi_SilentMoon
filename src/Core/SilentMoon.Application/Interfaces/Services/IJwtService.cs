using SilentMoon.Application.DTOs.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Services
{
    public interface IJwtService
    {
        JwtTokenDto GenerateToken(string userId, string email);
        ClaimsPrincipal ValidateToken(string token);
        string GenerateRefreshToken();

        //string HandleRefreshToken(string userId);

    }
}
