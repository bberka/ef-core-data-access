using EntityFrameworkCore.Ext.Abstractions.Collections;
using EntityFrameworkCore.Ext.Collections;

namespace EntityFrameworkCore.Ext.Extensions;

public static class PagedListExtensions
{
  public static IPagedList<T> ToPagedList<T>(this IList<T> source, int? pageIndex, int? pageSize, int totalCount) {
    return new PagedList<T>(source, pageIndex, pageSize, totalCount);
  }

  public static Task<IPagedList<T>> ToPagedListAsync<T>(this Task<IList<T>> source, int? pageIndex, int? pageSize, int totalCount, CancellationToken cancellationToken = default) {
    return source.Then<IList<T>, IPagedList<T>>(result => new PagedList<T>(result, pageIndex, pageSize, totalCount), cancellationToken);
  }
}