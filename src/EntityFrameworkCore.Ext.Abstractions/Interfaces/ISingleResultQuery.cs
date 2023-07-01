
namespace EntityFrameworkCore.Ext.Abstractions.Interfaces
{
    public interface ISingleResultQuery<T> : IQuery<T> where T : class
    { }

    public interface ISingleResultQuery<T, TResult> : IQuery<T, TResult> where T : class
    { }
}
