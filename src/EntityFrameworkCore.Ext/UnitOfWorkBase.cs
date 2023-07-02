using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Transactions;
using EntityFrameworkCore.Ext.Extensions;
using EntityFrameworkCore.Ext.Factories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using IsolationLevel = System.Data.IsolationLevel;

namespace EntityFrameworkCore.Ext;

public abstract class UnitOfWorkBase<T> : IUnitOfWorkBase where T : DbContext
{
  #region Ctor

  /// <param name="dbContext">Injected</param>
  protected UnitOfWorkBase(T dbContext) {
    DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), $"{nameof(dbContext)} cannot be null.");
  }

  #endregion Ctor

  #region Private Fields

  private IDbContextTransaction? _transaction;

  #endregion Private Fields


  #region IUnitOfWork Members

  protected DbContext DbContext { get; }

  public TimeSpan? Timeout {
    get {
      var commandTimeout = DbContext.Database.GetCommandTimeout();

      return commandTimeout.HasValue
               ? new TimeSpan?(TimeSpan.FromSeconds(commandTimeout.Value))
               : null;
    }
    set {
      var commandTimeout = value.HasValue
                             ? new int?(Convert.ToInt32(value.Value.TotalSeconds))
                             : null;

      DbContext.Database.SetCommandTimeout(commandTimeout);
    }
  }

  #endregion IUnitOfWork Members

  #region ISyncUnitOfWork Members

  public bool HasTransaction() {
    return _transaction != null;
  }

  public bool HasChanges() {
    bool autoDetectChangesEnabled;

    if (!(autoDetectChangesEnabled = DbContext.ChangeTracker.AutoDetectChangesEnabled)) DbContext.ChangeTracker.AutoDetectChangesEnabled = true;

    try {
      var hasChanges = DbContext.ChangeTracker.HasChanges();

      return hasChanges;
    }
    finally {
      DbContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesEnabled;
    }
  }

  public int SaveChanges(bool acceptAllChangesOnSuccess = true, bool ensureAutoHistory = false) {
    if (!HasChanges()) return 0;

    bool autoDetectChangesEnabled;
    if(!HasTransaction()) BeginTransaction();
    if (!(autoDetectChangesEnabled = DbContext.ChangeTracker.AutoDetectChangesEnabled)) DbContext.ChangeTracker.AutoDetectChangesEnabled = true;

    try {
      if (ensureAutoHistory) DbContext.EnsureAutoHistory();
      
      var res = DbContext.SaveChanges(acceptAllChangesOnSuccess);
      if(res != 0) Commit();
      return res;
    }
    catch (Exception ex) {
      Rollback();
      throw;
    }
    finally {
      DbContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesEnabled;
    }
  }

  public void DiscardChanges() {
    var dbEntityEntries = DbContext.ChangeTracker.Entries();

    foreach (var dbEntityEntry in dbEntityEntries) dbEntityEntry.State = EntityState.Detached;
  }

  protected void UseTransaction(DbTransaction transaction, Guid? transactionId = null) {
    if (transaction == null) throw new ArgumentNullException(nameof(transaction), $"{nameof(transaction)} cannot be null.");

    if (_transaction != null) throw new InvalidOperationException("There's already an active transaction.");

    _transaction = !transactionId.HasValue
                     ? DbContext.Database.UseTransaction(transaction)
                     : DbContext.Database.UseTransaction(transaction, transactionId.Value);
  }

  protected void EnlistTransaction(Transaction transaction) {
    if (transaction == null) throw new ArgumentNullException(nameof(transaction), $"{nameof(transaction)} cannot be null.");

    DbContext.Database.EnlistTransaction(transaction);
  }

  protected Transaction? GetEnlistedTransaction() {
    return DbContext.Database.GetEnlistedTransaction();
  }

  protected void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
    if (_transaction != null) throw new InvalidOperationException("There's already an active transaction.");

    _transaction = DbContext.Database.BeginTransaction(isolationLevel);
  }

  protected void Commit() {
    try {
      if (_transaction == null) throw new InvalidOperationException("There's no active transaction.");

      _transaction.Commit();
    }
    catch {
      Rollback();
      throw;
    }
    finally {
      DisposeTransaction();
    }
  }

  protected void Rollback() {
    try {
      _transaction?.Rollback();
    }
    catch {
      // ignored
    }
    finally {
      DisposeTransaction();
    }
  }

  public int ExecuteSqlCommand(string sql, params object[] parameters) {
    if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentException($"{nameof(sql)} cannot be null or white-space.", nameof(sql));

    var affectedRows = DbContext.Database.ExecuteSqlRaw(sql, parameters);

    return affectedRows;
  }

  public IList<T> FromSql<T>(string sql, params object[] parameters) where T : class {
    if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentException($"{nameof(sql)} cannot be null or white-space.", nameof(sql));

    var dbSet = DbContext.Set<T>();

    var entities = dbSet.FromSqlRaw(sql, parameters).ToList();

    return entities;
  }

  public void ChangeDatabase(string database) {
    if (string.IsNullOrWhiteSpace(database)) throw new ArgumentException($"{nameof(database)} cannot be null or white-space.", nameof(database));

    var dbConnection = DbContext.Database.GetDbConnection();

    if (dbConnection.State.HasFlag(ConnectionState.Open))
      dbConnection.ChangeDatabase(database);
    else
      dbConnection.ConnectionString = Regex.Replace(dbConnection.ConnectionString.Replace(" ", string.Empty), @"(?<=[Dd]atabase=)\w+(?=;)", database, RegexOptions.Singleline);

    var entityTypes = DbContext.Model.GetEntityTypes();

    foreach (var entityType in entityTypes)
      if (entityType is IConventionEntityType conventionEntityType)
        conventionEntityType.SetSchema(database);
  }

  public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback) {
    if (rootEntity == null) throw new ArgumentNullException(nameof(rootEntity), $"{nameof(rootEntity)} cannot be null.");

    if (callback == null) throw new ArgumentNullException(nameof(callback), $"{nameof(callback)} cannot be null.");

    DbContext.ChangeTracker.TrackGraph(rootEntity, callback);
  }

  public void TrackGraph<TState>(object rootEntity, TState state, Func<EntityEntryGraphNode<TState>, bool> callback) {
    if (rootEntity == null) throw new ArgumentNullException(nameof(rootEntity), $"{nameof(rootEntity)} cannot be null.");

    if (callback == null) throw new ArgumentNullException(nameof(callback), $"{nameof(callback)} cannot be null.");

    DbContext.ChangeTracker.TrackGraph<TState>(rootEntity, state, callback);
  }

  #endregion ISyncUnitOfWork Members

  #region IAsyncUnitOfWork Members

  public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, bool ensureAutoHistory = false, CancellationToken cancellationToken = default) {
    if (!HasChanges()) return await Task.FromResult(0).ConfigureAwait(false);

    bool autoDetectChangesEnabled;

    if (!(autoDetectChangesEnabled = DbContext.ChangeTracker.AutoDetectChangesEnabled)) DbContext.ChangeTracker.AutoDetectChangesEnabled = true;

    try {
      if (ensureAutoHistory) DbContext.EnsureAutoHistory();

      return await DbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
    finally {
      DbContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesEnabled;
    }
  }

  protected async Task UseTransactionAsync(DbTransaction transaction, Guid? transactionId = null, CancellationToken cancellationToken = default) {
    if (transaction == null) throw new ArgumentNullException(nameof(transaction), $"{nameof(transaction)} cannot be null.");

    if (_transaction != null) throw new InvalidOperationException("There's already an active transaction.");

    _transaction = !transactionId.HasValue
                     ? await DbContext.Database.UseTransactionAsync(transaction, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)
                     : await DbContext.Database.UseTransactionAsync(transaction, transactionId.Value, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
  }

  protected async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default) {
    if (_transaction != null) throw new InvalidOperationException("There's already an active transaction.");

    _transaction = await DbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
  }

  protected async Task CommitAsync(CancellationToken cancellationToken = default) {
    try {
      if (_transaction == null) throw new InvalidOperationException("There's no active transaction.");

      await _transaction.CommitAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
    catch {
      using (var source = new CancellationTokenSource()) {
        await RollbackAsync(source.Token).ConfigureAwait(false);
      }

      throw;
    }
    finally {
      await DisposeTransactionAsync().ConfigureAwait(false);
    }
  }

  protected async Task RollbackAsync(CancellationToken cancellationToken = default) {
    try {
      if (_transaction != null) await _transaction.RollbackAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }
    catch {
      // ignored
    }
    finally {
      await DisposeTransactionAsync().ConfigureAwait(false);
    }
  }

  public async Task<IList<T>> FromSqlAsync<T>(string sql, IEnumerable<object> parameters = null, CancellationToken cancellationToken = default) where T : class {
    if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentException($"{nameof(sql)} cannot be null or white-space.", nameof(sql));

    var dbSet = DbContext.Set<T>();

    var entities = await dbSet.FromSqlRaw(sql, parameters ?? Enumerable.Empty<object>()).ToListAsync(cancellationToken).Then<List<T>, IList<T>>(result => result, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

    return entities;
  }

  public async Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters = null, CancellationToken cancellationToken = default) {
    if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentException($"{nameof(sql)} cannot be null or white-space.", nameof(sql));

    var affectedRows = await DbContext.Database.ExecuteSqlRawAsync(sql, parameters ?? Enumerable.Empty<object>(), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

    return affectedRows;
  }

  #endregion IAsyncUnitOfWork Members

  #region Public Methods

  public static int SaveChanges(bool useTransaction = true, TimeSpan? timeout = null, bool acceptAllChangesOnSuccess = true, bool ensureAutoHistory = false, params IUnitOfWorkBase[] unitOfWorks) {
    if (!(unitOfWorks?.Any() ?? false)) return 0;

    var count = 0;

    
    if (useTransaction) {
      using var transactionScope = TransactionScopeFactory.CreateTransactionScope(timeout: timeout ?? TransactionManager.MaximumTimeout);

      SaveChangesInternal();

      transactionScope.Complete();
    }
    else {
      SaveChangesInternal();
    }

    return count;
    
    void SaveChangesInternal() {
      foreach (var unitOfWork in unitOfWorks) count += unitOfWork.SaveChanges(acceptAllChangesOnSuccess, ensureAutoHistory);
    }

  }

  public static async Task<int> SaveChangesAsync(bool useTransaction = true, TimeSpan? timeout = null, bool acceptAllChangesOnSuccess = true, bool ensureAutoHistory = false, CancellationToken cancellationToken = default, params IUnitOfWorkBase[] unitOfWorks) {
    if (!(unitOfWorks?.Any() ?? false)) return await Task.FromResult(0).ConfigureAwait(false);

    var count = 0;

    async Task SaveChangesAsyncInternal() {
      foreach (var unitOfWork in unitOfWorks) count += await unitOfWork.SaveChangesAsync(acceptAllChangesOnSuccess, ensureAutoHistory, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
    }

    if (useTransaction) {
      using var transactionScope = TransactionScopeFactory.CreateTransactionScope(timeout: timeout ?? TransactionManager.MaximumTimeout, transactionScopeAsyncFlowOption: TransactionScopeAsyncFlowOption.Enabled);

      await SaveChangesAsyncInternal().ConfigureAwait(false);

      transactionScope.Complete();
    }
    else {
      await SaveChangesAsyncInternal().ConfigureAwait(false);
    }

    return count;
  }

  #endregion Public Methods

  #region Private Methods

  private void DisposeTransaction() {
    if (_transaction != null) {
      _transaction.Dispose();
      _transaction = null;
    }
  }

  private async Task DisposeTransactionAsync() {
    if (_transaction != null) {
      await _transaction.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
      _transaction = null;
    }
  }

  #endregion Private Methods

  #region IDisposable Members

  private bool _disposed;

  protected virtual void Dispose(bool disposing) {
    if (_disposed) return;
    if (disposing) {
      DisposeTransaction();
      if (DbContext.Database.IsRelational()) {
        var connection = DbContext.Database.GetDbConnection();
        if (connection?.State != ConnectionState.Closed) connection?.Close();
        DbContext.Dispose();
      }
    }
    _disposed = true;
  }

  public void Dispose() {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  #endregion IDisposable Members
}