using System.Diagnostics.CodeAnalysis;
using Shoka.Core.Infrastructure.Uow;
using Shoka.Domain.Entities;

namespace Shoka.Domain.Repositories;

public abstract class CommandRepository<TEntity> : ICommandRepository<TEntity> where TEntity : class, IEntity
{
    private readonly IUnitOfWork _uow;

    protected CommandRepository([NotNull] IUnitOfWork uow)
    {
        _uow = uow;
    }

    
    public abstract Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    public abstract Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    public Task DeleteManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task InsertManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await InsertAsync(entity);
        }
        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    public Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateManyAsync([NotNull] IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        _uow.SaveChangesAsync();
        return Task.CompletedTask;
    }

}