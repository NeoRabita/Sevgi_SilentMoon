using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

public interface IUow : IDisposable
{
     IGenericRepository<ApplicationUser> UserRepository { get; }
    IGenericRepository<RefreshToken> RefreshTokenRepository { get; }
    IGenericRepository<Topic> TopicRepository { get; }
    IGenericRepository<UserTopic> UserTopicRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}