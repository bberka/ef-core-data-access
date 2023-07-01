namespace EntityFrameworkCore.Ext.Abstractions.Interfaces
{
    public interface IRepository : IDisposable
    { }

    public interface IRepository<T> : IRepository, ISyncRepository<T>, IAsyncRepository<T>, IDisposable where T : class
    { }
}
