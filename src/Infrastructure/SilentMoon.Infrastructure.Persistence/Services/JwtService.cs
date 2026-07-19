using Microsoft.Extensions.Configuration;
using SilentMoon.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using SilentMoon.Application.DTOs.JWT;
using SilentMoon.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Options;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Domain.Entities;

namespace SilentMoon.Infrastructure.Persistence.Services
{
    public class JwtService : IJwtService
    {
        private readonly APIAppSettings _settings;
        public JwtService(IOptions<APIAppSettings> options)
        {
            _settings = options.Value;
        }
        public RefreshToken GenerateRefreshToken(string userId)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var expires = DateTime.UtcNow.AddDays(_settings.JWTSettings.RefreshTokenDuration);
            return new RefreshToken
            {
                Token = token,
                Expires = expires,
                UserId = userId,
                Created = DateTime.UtcNow,
                CreatedByIp = "127.0.0.1",
                IsRevoked = false
            };
        }

        public RefreshToken UpdateRefreshToken(RefreshToken refreshToken)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var expires = DateTime.UtcNow.AddDays(_settings.JWTSettings.RefreshTokenDuration);
            refreshToken.Expires = expires;
            refreshToken.Token = token;
            return refreshToken;
        }

        public JwtTokenDto GenerateToken(string userId, string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JWTSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_settings.JWTSettings.DurationInMinutes);

            var token = new JwtSecurityToken(
                issuer: _settings.JWTSettings.Issuer,
                audience: _settings.JWTSettings.Audience,
                claims: claims,
                expires: expiresAt.UtcDateTime,
                signingCredentials: credentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new JwtTokenDto(tokenString, expiresAt);

        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_settings.JWTSettings.Key);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _settings.JWTSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _settings.JWTSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
