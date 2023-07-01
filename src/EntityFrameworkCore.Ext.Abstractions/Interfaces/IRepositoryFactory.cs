using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Ext.Abstractions.Interfaces
{
    public interface IRepositoryFactory
    {
        T CustomRepository<T>() where T : class;
        IRepository<T> Repository<T>() where T : class;
    }

    public interface IRepositoryFactory<T> : IRepositoryFactory where T : DbContext
    { }
}
