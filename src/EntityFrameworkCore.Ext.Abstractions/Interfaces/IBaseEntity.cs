using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Ext.Abstractions.Interfaces;

public interface IBaseEntity
{
  [Key]
  public Guid Guid { get;  }
  public DateTime RegisterDate { get;  }
}