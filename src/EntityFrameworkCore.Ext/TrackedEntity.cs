using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityFrameworkCore.Ext;

public class TrackedEntity
{
  public TrackedEntity(EntityEntry entityEntry) {
    EntityEntry = entityEntry ?? throw new ArgumentNullException(nameof(entityEntry), $"{nameof(entityEntry)} cannot be null.");
    EntityState = entityEntry.State;
  }

  public EntityEntry EntityEntry { get; }
  public EntityState EntityState { get; }
}