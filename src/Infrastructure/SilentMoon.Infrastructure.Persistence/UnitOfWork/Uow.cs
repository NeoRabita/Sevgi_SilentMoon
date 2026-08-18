using Microsoft.EntityFrameworkCore.Storage;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Infrastructure.Persistence.Contexts;
using System.Threading.Tasks;
using System.Threading;
using SilentMoon.Domain.Entities;

public class Uow : IUow
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    public IGenericRepository<ApplicationUser> UserRepository { get; }
    public IGenericRepository<RefreshToken> RefreshTokenRepository { get; }

    public ITopicRepository TopicRepository { get; }
    public IGenericRepository<UserTopic> UserTopicRepository { get; }

    public IGenericRepository<Translation> TranslationRepository { get; }

    public IGenericRepository<Course> CourseRepository {  get; }

    public IGenericRepository<Category> CategoryRepository {  get; }

    public Uow(
        AppDbContext context, IGenericRepository<ApplicationUser> userRepository, IGenericRepository<RefreshToken> refreshTokenRepository, ITopicRepository topicRepository, IGenericRepository<UserTopic> userTopicRepository)
    {
        _context = context;
        UserRepository = userRepository;
        RefreshTokenRepository = refreshTokenRepository;
        TopicRepository = topicRepository;
        UserTopicRepository = userTopicRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null) return;
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        _transaction?.Dispose();
    }
}