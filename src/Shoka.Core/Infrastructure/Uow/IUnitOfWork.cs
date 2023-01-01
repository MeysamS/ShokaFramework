namespace Shoka.Core.Infrastructure.Uow;

public interface IUnitOfWork : IDisposable
{
    int SaveChanges(bool acceptAllChangesOnSuccess);
    int SaveChanges();
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new());
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());

}