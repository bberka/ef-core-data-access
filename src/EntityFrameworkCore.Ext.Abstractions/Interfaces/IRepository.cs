namespace EntityFrameworkCore.Ext.Abstractions.Interfaces
{
    public interface IRepository<T> : ISyncRepository<T>, IAsyncRepository<T> where T : class
    { }
}
