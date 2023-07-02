namespace EntityFrameworkCore.Ext;

// public class PooledUnitOfWork<T> : UnitOfWorkBase,
//                                    IPooledUnitOfWork<T> where T : DbContext
// {
//   #region Ctor
//
//   public PooledUnitOfWork(IDbContextFactory<T> dbContextFactory)
//     : base(CreateDbContext(dbContextFactory)) {
//     DbContextFactory = dbContextFactory;
//     
//   }
//
//   #endregion Ctor
//
//   #region IPooledUnitOfWork Members
//
//   public IDbContextFactory<T> DbContextFactory { get; }
//
//   #endregion IPooledUnitOfWork Members
//
//   #region Private Methods
//
//   private static T CreateDbContext(IDbContextFactory<T> dbContextFactory) {
//     if (dbContextFactory == null) throw new ArgumentNullException(nameof(dbContextFactory), $"{nameof(dbContextFactory)} cannot be null.");
//
//     var dbContext = dbContextFactory.CreateDbContext();
//
//     return dbContext;
//   }
//
//   #endregion Private Methods
// }