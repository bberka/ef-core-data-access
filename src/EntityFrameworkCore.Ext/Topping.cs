namespace EntityFrameworkCore.Ext;

public class Topping : ITopping
{
  internal Topping() { }

  public int? TopRows { get; internal set; }
  public bool IsEnabled => TopRows > 0;
}