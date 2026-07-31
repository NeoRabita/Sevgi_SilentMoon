using SilentMoon.Application.Common.User;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
using System;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly ICurrentUser _currentUser;
    private readonly IGenericRepository<ApplicationUser> _repository;

    public UserService(
        ICurrentUser currentUser,
        IGenericRepository<ApplicationUser> repository)
    {
        _currentUser = currentUser;
        _repository = repository;
    }

    public async Task<ApplicationUser> GetCurrentUserAsync()
    {
        if (string.IsNullOrWhiteSpace(_currentUser.Email))
            throw new UnauthorizedAccessException();

        var user = await _repository.GetAsync(x => x.Email == _currentUser.Email);

        if (user == null)
            throw new UnauthorizedAccessException();

        return user;
    }
}