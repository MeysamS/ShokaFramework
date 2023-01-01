namespace Shoka.Core.Infrastructure.Uow;

public interface IDbTransaction : IDisposable
{
    Task BeginTransaction();
    Task RollbackTransaction();
    Task CommitTransaction();
}