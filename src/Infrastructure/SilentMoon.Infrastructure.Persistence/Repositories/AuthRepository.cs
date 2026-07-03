using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Domain.Entities;
using SilentMoon.Infrastructure.Persistence.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDapper _dapper;

        public AuthRepository(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task ActivateApplicationUser(string id)
        {
            var sql = "UPDATE ApplicationUsers SET IsEmailConfirmed = 1 WHERE Id = :Id";
            await _dapper.ExecuteAsync(sql, new { Id = id });

        }

        public async Task<ApplicationUser> CreateApplicationUserAsync(RegisterRequest registerRequest)
        {
            var passwordHash =BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            var userDto = new ApplicationUserDbDTO
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = registerRequest.Name,
                Email = registerRequest.Email,
                PasswordHash = passwordHash,
            };

            var sql = "INSERT INTO ApplicationUsers (Id, FirstName, Email, PasswordHash, IsEmailConfirmed) VALUES (:Id, :FirstName, :Email, :PasswordHash, :IsEmailConfirmed)";
            await _dapper.ExecuteAsync(sql, userDto);
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = registerRequest.Name,
                //LastName = registerRequest.Surname,
                Email = registerRequest.Email,
                PasswordHash = passwordHash,
                IsEmailConfirmed = false
            };

            return user;
        }

        public async Task<ApplicationUser> GetApplicationUserByEmailAsync(string email)
        {
            var sql = "SELECT * FROM ApplicationUsers WHERE Email = :Email";
            var user = await _dapper.GetAsync<ApplicationUser>(sql, new { Email = email });
            return user;
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            var sql = "INSERT INTO RefreshTokens (Id, Token, UserId, Expires, Created,CreatedByIp,IsRevoked) VALUES (refresh_token_seq.NEXTVAL,, :Token, :UserId, :Expires, :Created, :CreatedByIp, :IsRevoked)";
            await _dapper.ExecuteAsync(sql, refreshToken);
            var updateSql = "UPDATE ApplicationUsers SET RefreshTokenId = :RefreshTokenId WHERE Id = :UserId";
            await _dapper.ExecuteAsync(updateSql, new
            {
                RefreshTokenId = refreshToken.Id,
                UserId = refreshToken.UserId
            });
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var sql = "SELECT * FROM ApplicationUsers WHERE Email = :Email";
            var user = await _dapper.GetAsync<ApplicationUser>(sql, new { Email = email });
            return user != null;
        }
    }
}
