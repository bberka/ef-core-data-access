using System.Diagnostics;
using EntityFrameworkCore.Ext.Serialization;
using Newtonsoft.Json;

namespace EntityFrameworkCore.Ext;

public sealed class AutoHistoryOptions
{
  private static readonly Lazy<AutoHistoryOptions> AutoHistoryOptionsFactory = new(() => new AutoHistoryOptions(), true);

  private AutoHistoryOptions() { }

  public static AutoHistoryOptions Instance => AutoHistoryOptionsFactory.Value;

  public int RowIdMaxLength { get; set; } = AutoHistoryOptionsDefaults.RowIdMaxLength;
  public int TableNameMaxLength { get; set; } = AutoHistoryOptionsDefaults.TableNameMaxLength;
  public bool LimitChangedLength { get; set; } = AutoHistoryOptionsDefaults.LimitChangedLength;
  public int? ChangedMaxLength { get; set; }
  public string AutoHistoryTableName { get; set; } = AutoHistoryOptionsDefaults.AutoHistoryTableName;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  internal JsonSerializer JsonSerializer { get; set; }

  public JsonSerializerSettings JsonSerializerSettings { get; set; } = AutoHistorySerialization.DefaultSettings;
}