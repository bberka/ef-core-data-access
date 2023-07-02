using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Ext.Abstractions.Interfaces
{
    public interface IAsyncUnitOfWork :  IDisposable
    {
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, bool ensureAutoHistory = false, CancellationToken cancellationToken = default);
        // Task UseTransactionAsync(DbTransaction transaction, Guid? transactionId = null, CancellationToken cancellationToken = default);
        // Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
        // Task CommitAsync(CancellationToken cancellationToken = default);
        // Task RollbackAsync(CancellationToken cancellationToken = default);
        Task<IList<T>> FromSqlAsync<T>(string sql, IEnumerable<object> parameters = null, CancellationToken cancellationToken = default) where T : class;
        Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters = null, CancellationToken cancellationToken = default);
    }

    public interface IAsyncUnitOfWork<T> : IAsyncUnitOfWork,  IDisposable where T : DbContext
    { }
}
