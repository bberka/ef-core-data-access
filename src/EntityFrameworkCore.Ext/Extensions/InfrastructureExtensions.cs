using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Ext.Extensions;

internal static class InfrastructureExtensions
{
  public static TService TryGetService<TService>(this IInfrastructure<IServiceProvider> accessor) where TService : class {
    if (accessor == null) throw new ArgumentNullException(nameof(accessor), $"{nameof(accessor)} cannot be null.");

    var service = accessor.Instance.GetService(typeof(TService))
                  ?? accessor.TryGetApplicationServiceProvider()?.GetService(typeof(TService));

    return (TService)service;
  }

  public static IServiceProvider TryGetApplicationServiceProvider(this IInfrastructure<IServiceProvider> accessor) {
    if (accessor == null) throw new ArgumentNullException(nameof(accessor), $"{nameof(accessor)} cannot be null.");

    var applicationServiceProvider = accessor.Instance.GetService<IDbContextOptions>()
                                             ?.Extensions
                                             ?.OfType<CoreOptionsExtension>()
                                             ?.FirstOrDefault()
                                             ?.ApplicationServiceProvider;

    return applicationServiceProvider;
  }
}