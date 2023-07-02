using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Ext;

public abstract class BaseEntity : IEntity
{
  [Key]
  public Guid Guid { get; set; }
  public DateTime RegisterDate { get; set; }
}