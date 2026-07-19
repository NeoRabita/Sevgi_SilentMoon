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

        public async Task<Result<ApplicationUser>> GetCurrentUserAsync()
        {
            var userEmail = _httpContextAccessor.HttpContext?.User?
     .FindFirst(ClaimTypes.Email)?
     .Value;
            var user = await _genericRepository.GetAsync(u => u.Email == userEmail);
            if (user is null)
            {
                return UserErrors.NotFoundByEmail;
            }
            return user;

        }
    }
}
