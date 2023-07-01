
namespace EntityFrameworkCore.Ext.Abstractions.Interfaces
{
    public interface IPaging
    {
        int? PageIndex { get; }
        int? PageSize { get; }
        int TotalCount { get; }
        bool IsEnabled { get; }
    }
}
