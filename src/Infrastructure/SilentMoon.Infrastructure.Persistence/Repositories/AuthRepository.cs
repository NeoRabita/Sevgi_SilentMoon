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

        public async Task<bool> UserExistsAsync(string email)
        {
            var sql = "SELECT * FROM ApplicationUsers WHERE Email = :Email";
            var user = await _dapper.GetAsync<ApplicationUser>(sql, new { Email = email });
            return user != null;
        }
    }
}
