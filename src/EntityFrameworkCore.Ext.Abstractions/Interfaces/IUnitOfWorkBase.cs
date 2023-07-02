using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Ext.Abstractions.Interfaces
{
    public interface IUnitOfWorkBase : ISyncUnitOfWork, IAsyncUnitOfWork, IDisposable
    {
        TimeSpan? Timeout { get; set; }
    }

    public interface IUnitOfWorkBase<T> : IUnitOfWorkBase, ISyncUnitOfWork<T>, IAsyncUnitOfWork<T>, IDisposable where T : DbContext
    { }

    public interface IPooledUnitOfWorkBase<T> : IUnitOfWorkBase<T>, IDisposable where T : DbContext
    {
        IDbContextFactory<T> DbContextFactory { get; }
    }
}
