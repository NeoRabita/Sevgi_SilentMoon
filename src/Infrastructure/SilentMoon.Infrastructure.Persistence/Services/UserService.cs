using Microsoft.AspNetCore.Http;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
using SilentMoon.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericRepository<ApplicationUser> _genericRepository;

        public UserService(IHttpContextAccessor httpContextAccessor, IGenericRepository<ApplicationUser> genericRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _genericRepository = genericRepository;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var principal = _httpContextAccessor.HttpContext?.User;

            if (principal?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email))
                throw new UnauthorizedAccessException("Email claim not found.");

            var user = await _genericRepository.GetAsync(u => u.Email == email);

            if (user is null)
                throw new UnauthorizedAccessException("User not found.");

            return user;
        }
    }
}
