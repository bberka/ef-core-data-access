using System.Linq.Expressions;

namespace EntityFrameworkCore.Ext;

public class Sorting<T> : Sorting,
                          ISorting<T>
{
  internal Sorting() { }

  public Expression<Func<T, object>> KeySelector { get; internal set; }
}

public class Sorting : ISorting
{
  internal Sorting() { }

  public string FieldName { get; internal set; }
  public SortDirection SortDirection { get; internal set; }
}