using SilentMoon.Application.DTOs.JWT;
using SilentMoon.Domain.Entities;
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
        RefreshToken GenerateRefreshToken(string userId);
        RefreshToken UpdateRefreshToken(RefreshToken refreshToken);
        //string HandleRefreshToken(string userId);

    }
}
