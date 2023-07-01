using EntityFrameworkCore.Ext.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EntityFrameworkCore.Ext.Extensions;

public static class ServiceCollectionExtensions
{
    
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) {
    if (services == null) throw new ArgumentNullException(nameof(services), $"{nameof(services)} cannot be null.");

    switch (serviceLifetime) {
      case ServiceLifetime.Singleton: {
        services.TryAddSingleton<IRepositoryFactory, UnitOfWorkBase>();
        services.TryAddSingleton<IUnitOfWork, UnitOfWorkBase>();
      }
        break;
      case ServiceLifetime.Scoped: {
        services.TryAddScoped<IRepositoryFactory, UnitOfWorkBase>();
        services.TryAddScoped<IUnitOfWork, UnitOfWorkBase>();
      }
        break;
      case ServiceLifetime.Transient: {
        services.TryAddTransient<IRepositoryFactory, UnitOfWorkBase>();
        services.TryAddTransient<IUnitOfWork, UnitOfWorkBase>();
      }
        break;
    }

    return services;
  }
    public static IServiceCollection AddUnitOfWork<T>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where T : UnitOfWorkBase {
        if (services == null) throw new ArgumentNullException(nameof(services), $"{nameof(services)} cannot be null.");

        switch (serviceLifetime) {
            case ServiceLifetime.Singleton: {
                services.TryAddSingleton<IUnitOfWork, T>();
            }
                break;
            case ServiceLifetime.Scoped: {
                services.TryAddScoped<IUnitOfWork, T>();
            }
                break;
            case ServiceLifetime.Transient: {
                services.TryAddTransient<IUnitOfWork, T>();
            }
                break;
        }

        return services;
    }
    public static IServiceCollection AddUnitOfWorkWithDbContext<T>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where T : DbContext {
    if (services == null) throw new ArgumentNullException(nameof(services), $"{nameof(services)} cannot be null.");

    switch (serviceLifetime) {
      case ServiceLifetime.Singleton: {
        services.TryAddSingleton<IRepositoryFactory<T>, UnitOfWorkBase<T>>();
        services.TryAddSingleton<IUnitOfWork<T>, UnitOfWorkBase<T>>();
      }
        break;
      case ServiceLifetime.Scoped: {
        services.TryAddScoped<IRepositoryFactory<T>, UnitOfWorkBase<T>>();
        services.TryAddScoped<IUnitOfWork<T>, UnitOfWorkBase<T>>();
      }
        break;
      case ServiceLifetime.Transient: {
        services.TryAddTransient<IRepositoryFactory<T>, UnitOfWorkBase<T>>();
        services.TryAddTransient<IUnitOfWork<T>, UnitOfWorkBase<T>>();
      }
        break;
    }

    return services;
  }

  public static IServiceCollection AddPooledUnitOfWork<T>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where T : DbContext {
    if (services == null) throw new ArgumentNullException(nameof(services), $"{nameof(services)} cannot be null.");

    switch (serviceLifetime) {
      case ServiceLifetime.Singleton: {
        services.TryAddSingleton<IRepositoryFactory<T>, PooledUnitOfWork<T>>();
        services.TryAddSingleton<IUnitOfWork<T>, PooledUnitOfWork<T>>();
        services.TryAddSingleton<IPooledUnitOfWork<T>, PooledUnitOfWork<T>>();
      }
        break;
      case ServiceLifetime.Scoped: {
        services.TryAddScoped<IRepositoryFactory<T>, PooledUnitOfWork<T>>();
        services.TryAddScoped<IUnitOfWork<T>, PooledUnitOfWork<T>>();
        services.TryAddScoped<IPooledUnitOfWork<T>, PooledUnitOfWork<T>>();
      }
        break;
      case ServiceLifetime.Transient: {
        services.TryAddTransient<IRepositoryFactory<T>, PooledUnitOfWork<T>>();
        services.TryAddTransient<IUnitOfWork<T>, PooledUnitOfWork<T>>();
        services.TryAddTransient<IPooledUnitOfWork<T>, PooledUnitOfWork<T>>();
      }
        break;
    }

    return services;
  }
  public static IServiceCollection AddCustomRepository<TService, TImplementation>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    where TService : class, IRepository
    where TImplementation : class, TService {
    if (services == null) throw new ArgumentNullException(nameof(services), $"{nameof(services)} cannot be null.");

    if (!(typeof(TImplementation).BaseType?.IsGenericType(typeof(Repository<>)) ?? false)) throw new ArgumentException("Implementation constraint has not been satisfied.");

    switch (serviceLifetime) {
      case ServiceLifetime.Singleton: {
        services.TryAddSingleton<TService, TImplementation>();
      }
        break;
      case ServiceLifetime.Scoped: {
        services.TryAddScoped<TService, TImplementation>();
      }
        break;
      case ServiceLifetime.Transient: {
        services.TryAddTransient<TService, TImplementation>();
      }
        break;
    }

    return services;
  }

  public static IServiceCollection AddRepository<TService, TImplementation>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    where TService : class, IRepository
    where TImplementation : class, TService {
    if (services == null) throw new ArgumentNullException(nameof(services), $"{nameof(services)} cannot be null.");

    if (!typeof(TImplementation).IsGenericType(typeof(Repository<>))) throw new ArgumentException("Implementation constraint has not been satisfied.");

    switch (serviceLifetime) {
      case ServiceLifetime.Singleton: {
        services.TryAddSingleton<TService, TImplementation>();
      }
        break;
      case ServiceLifetime.Scoped: {
        services.TryAddScoped<TService, TImplementation>();
      }
        break;
      case ServiceLifetime.Transient: {
        services.TryAddTransient<TService, TImplementation>();
      }
        break;
    }

    return services;
  }
}

