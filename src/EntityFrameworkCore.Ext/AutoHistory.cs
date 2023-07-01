﻿using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Ext;

public class AutoHistory
{
  public long Id { get; set; }
  public string RowId { get; set; }
  public string TableName { get; set; }
  public string Changed { get; set; }
  public EntityState Kind { get; set; }
  public DateTime Created { get; set; } = DateTime.UtcNow;
}