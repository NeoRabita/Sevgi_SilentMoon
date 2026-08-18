using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

public interface IUow : IDisposable
{
     IGenericRepository<ApplicationUser> UserRepository { get; }
    IGenericRepository<RefreshToken> RefreshTokenRepository { get; }
    ITopicRepository TopicRepository { get; }
    IGenericRepository<UserTopic> UserTopicRepository { get; }
    IGenericRepository<Translation> TranslationRepository { get; }
    IGenericRepository<Course> CourseRepository { get; }
    IGenericRepository<Category> CategoryRepository { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}