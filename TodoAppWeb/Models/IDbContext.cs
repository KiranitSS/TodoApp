using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace TodoAppWeb.Models
{
    public interface IDbContext :
    IInfrastructure<IServiceProvider>
    {
        DatabaseFacade Database { get; }

        ChangeTracker ChangeTracker { get; }

        public IModel Model { get; }

        public DbContextId ContextId { get; }

        public DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        public DbSet<TEntity> Set<TEntity>(string name)
            where TEntity : class;

        public int SaveChanges();

        public int SaveChanges(bool acceptAllChangesOnSuccess);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public event EventHandler<SavingChangesEventArgs>? SavingChanges;

        public event EventHandler<SavedChangesEventArgs>? SavedChanges;

        public event EventHandler<SaveChangesFailedEventArgs>? SaveChangesFailed;

        public void Dispose();

        public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) 
            where TEntity : class;

        public EntityEntry Entry(object entity);

        public EntityEntry<TEntity> Add<TEntity>(TEntity entity)
            where TEntity : class;

        public EntityEntry Add(object entity);


        public EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
            where TEntity : class;

        public EntityEntry Attach(object entity);

        public EntityEntry<TEntity> Update<TEntity>(TEntity entity)
            where TEntity : class;


        public EntityEntry Update(object entity);

        public EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
            where TEntity : class;


        public EntityEntry Remove(object entity);

        public void AddRange(params object[] entities);

        public void AddRange(IEnumerable<object> entities);

        public Task AddRangeAsync(params object[] entities);

        public void AttachRange(params object[] entities);

        public void AttachRange(IEnumerable<object> entities);

        public void UpdateRange(params object[] entities);

        public void UpdateRange(IEnumerable<object> entities);

        public void RemoveRange(params object[] entities);

        public void RemoveRange(IEnumerable<object> entities);

        public object? Find(Type entityType,
            params object?[]? keyValues);

        public TEntity? Find<TEntity>(
            params object?[]? keyValues)
            where TEntity : class;

        public ValueTask<object?> FindAsync(Type entityType,
            params object?[]? keyValues);

        public ValueTask<object?> FindAsync(Type entityType,
            object?[]? keyValues,
            CancellationToken cancellationToken);

        public ValueTask<TEntity?> FindAsync<TEntity>(params object?[]? keyValues)
            where TEntity : class;

        public IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression);
    }
}
