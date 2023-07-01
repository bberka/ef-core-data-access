namespace EntityFrameworkCore.Ext;

public class SingleResultQuery<T> : Query<T>,
                                    ISingleResultQuery<T> where T : class
{
  #region Ctor

  internal SingleResultQuery() { }

  #endregion Ctor

  public static ISingleResultQuery<T> New() {
    return new SingleResultQuery<T>();
  }
}

public class SingleResultQuery<T, TResult> : Query<T, TResult>,
                                             ISingleResultQuery<T, TResult> where T : class
{
  #region Ctor

  internal SingleResultQuery() { }

  #endregion Ctor

  public static ISingleResultQuery<T, TResult> New() {
    return new SingleResultQuery<T, TResult>();
  }
}