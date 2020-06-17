﻿using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.Repository.Extensions;
using EntityFrameworkCore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace EntityFrameworkCore.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Protected Properties

        protected DbContext DbContext { get; }
        protected DbSet<T> DbSet { get; }

        #endregion

        #region Ctor

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), $"{nameof(dbContext)} cannot be null.");
            DbSet = dbContext.Set<T>();
        }

        #endregion

        #region IQueryFactory<T> Members

        public virtual ISingleResultQuery<T> SingleResultQuery() => QueryBuilder.SingleResultQuery<T>.New();
        public virtual IMultipleResultQuery<T> MultipleResultQuery() => QueryBuilder.MultipleResultQuery<T>.New();

        public virtual ISingleResultQuery<T, TResult> SingleResultQuery<TResult>() => QueryBuilder.SingleResultQuery<T, TResult>.New();
        public virtual IMultipleResultQuery<T, TResult> MultipleResultQuery<TResult>() => QueryBuilder.MultipleResultQuery<T, TResult>.New();

        #endregion IQueryFactory<T> Members

        #region ISyncRepository<T> Members

        public virtual IList<T> Search(IQuery<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entities = queryable.ToList();

            return entities;
        }

        public virtual IList<TResult> Search<TResult>(IQuery<T, TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            if (query.Selector == null)
            {
                throw new ArgumentNullException(nameof(query.Selector), $"{nameof(query.Selector)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entities = queryable.ToList();

            return entities;
        }

        public virtual T SingleOrDefault(IQuery<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.SingleOrDefault();

            return entity;
        }

        public virtual TResult SingleOrDefault<TResult>(IQuery<T, TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            if (query.Selector == null)
            {
                throw new ArgumentNullException(nameof(query.Selector), $"{nameof(query.Selector)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.SingleOrDefault();

            return entity;
        }

        public virtual T FirstOrDefault(IQuery<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.FirstOrDefault();

            return entity;
        }

        public virtual TResult FirstOrDefault<TResult>(IQuery<T, TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            if (query.Selector == null)
            {
                throw new ArgumentNullException(nameof(query.Selector), $"{nameof(query.Selector)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.FirstOrDefault();

            return entity;
        }

        public virtual T LastOrDefault(IQuery<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.LastOrDefault();

            return entity;
        }

        public virtual TResult LastOrDefault<TResult>(IQuery<T, TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            if (query.Selector == null)
            {
                throw new ArgumentNullException(nameof(query.Selector), $"{nameof(query.Selector)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.LastOrDefault();

            return entity;
        }

        public virtual bool Any(Expression<Func<T, bool>> predicate = null)
        {
            var queryable = GetQueryable();

            var result = predicate == null ? queryable.Any() : queryable.Any(predicate);

            return result;
        }

        public virtual int Count(Expression<Func<T, bool>> predicate = null)
        {
            var queryable = GetQueryable();

            var result = predicate == null ? queryable.Count() : queryable.Count(predicate);

            return result;
        }

        public virtual long LongCount(Expression<Func<T, bool>> predicate = null)
        {
            var queryable = GetQueryable();

            var result = predicate == null ? queryable.LongCount() : queryable.LongCount(predicate);

            return result;
        }

        public virtual TResult Max<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector), $"{nameof(selector)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = predicate == null ? queryable.Max(selector) : queryable.Where(predicate).Max(selector);

            return result;
        }

        public virtual TResult Min<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector), $"{nameof(selector)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = predicate == null ? queryable.Min(selector) : queryable.Where(predicate).Min(selector);

            return result;
        }

        public virtual decimal Average(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate = null)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector), $"{nameof(selector)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = predicate == null ? queryable.Average(selector) : queryable.Where(predicate).Average(selector);

            return result;
        }

        public virtual decimal Sum(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate = null)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector), $"{nameof(selector)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = predicate == null ? queryable.Sum(selector) : queryable.Where(predicate).Sum(selector);

            return result;
        }

        public virtual T Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} cannot be null.");
            }

            DbSet.Add(entity);

            return entity;
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} cannot be null.");
            }

            if (!entities.Any())
            {
                return;
            }

            DbSet.AddRange(entities);
        }

        public virtual T Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} cannot be null.");
            }

            var entityEntry = DbContext.Entry(entity);

            if (entityEntry.State == EntityState.Detached)
            {
                entityEntry = DbSet.Attach(entity);
            }

            if (properties?.Any() ?? false)
            {
                foreach (var property in properties)
                {
                    PropertyEntry propertyEntry;

                    try
                    {
                        propertyEntry = entityEntry.Property(property);
                    }
                    catch { propertyEntry = null; }

                    if (propertyEntry != null)
                    {
                        propertyEntry.IsModified = true;
                    }
                    else
                    {
                        ReferenceEntry referenceEntry;

                        try
                        {
                            referenceEntry = entityEntry.Reference(property);
                        }
                        catch { referenceEntry = null; }

                        if (referenceEntry != null)
                        {
                            var referenceEntityEntry = referenceEntry.TargetEntry;

                            DbContext.Attach(referenceEntityEntry.Entity);

                            DbContext.Update(referenceEntityEntry.Entity);
                        }
                    }
                }
            }
            else
            {
                DbSet.Update(entity);
            }

            return entity;
        }

        public virtual int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> expression)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), $"{nameof(predicate)} cannot be null.");
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), $"{nameof(expression)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = queryable.Where(predicate).Update(expression);

            return result;
        }

        public virtual void UpdateRange(IEnumerable<T> entities, params Expression<Func<T, object>>[] properties)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} cannot be null.");
            }

            if (!entities.Any())
            {
                return;
            }

            foreach (var entity in entities)
            {
                Update(entity, properties);
            }
        }

        public virtual T Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} cannot be null.");
            }

            var entityEntry = DbContext.Entry(entity);

            if (entityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);

            return entity;
        }

        public virtual int Remove(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), $"{nameof(predicate)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = queryable.Where(predicate).Delete();

            return result;
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} cannot be null.");
            }

            if (!entities.Any())
            {
                return;
            }

            DbSet.RemoveRange(entities);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException($"{nameof(sql)} cannot be null or white-space.", nameof(sql));
            }

            var affectedRows = DbContext.Database.ExecuteSqlCommand(sql, parameters);

            return affectedRows;
        }

        public virtual IList<T> FromSql(string sql, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException($"{nameof(sql)} cannot be null or white-space.", nameof(sql));
            }

            var queryable = GetQueryable();

            var entities = queryable.FromSql(sql, parameters).ToList();

            return entities;
        }

        public virtual void ChangeTable(string table)
        {
            var entityType = DbContext.Model.FindEntityType(typeof(T));

            if (entityType?.Relational() is RelationalEntityTypeAnnotations relationalEntityType)
            {
                relationalEntityType.TableName = table;
            }
        }

        #endregion ISyncRepository<T> Members

        #region IAsyncRepository<T> Members

        public virtual Task<IList<T>> SearchAsync(IQuery<T> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entities = queryable.ToListAsync(cancellationToken).Then<List<T>, IList<T>>(result => result);

            return entities;
        }

        public virtual Task<IList<TResult>> SearchAsync<TResult>(IQuery<T, TResult> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            if (query.Selector == null)
            {
                throw new ArgumentNullException(nameof(query.Selector), $"{nameof(query.Selector)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entities = queryable.ToListAsync(cancellationToken).Then<List<TResult>, IList<TResult>>(result => result);

            return entities;
        }

        public virtual Task<T> SingleOrDefaultAsync(IQuery<T> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.SingleOrDefaultAsync(cancellationToken);

            return entity;
        }

        public virtual Task<TResult> SingleOrDefaultAsync<TResult>(IQuery<T, TResult> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            if (query.Selector == null)
            {
                throw new ArgumentNullException(nameof(query.Selector), $"{nameof(query.Selector)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.SingleOrDefaultAsync(cancellationToken);

            return entity;
        }

        public virtual Task<T> FirstOrDefaultAsync(IQuery<T> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.FirstOrDefaultAsync(cancellationToken);

            return entity;
        }

        public virtual Task<TResult> FirstOrDefaultAsync<TResult>(IQuery<T, TResult> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            if (query.Selector == null)
            {
                throw new ArgumentNullException(nameof(query.Selector), $"{nameof(query.Selector)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.FirstOrDefaultAsync(cancellationToken);

            return entity;
        }

        public virtual Task<T> LastOrDefaultAsync(IQuery<T> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.LastOrDefaultAsync(cancellationToken);

            return entity;
        }

        public virtual Task<TResult> LastOrDefaultAsync<TResult>(IQuery<T, TResult> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), $"{nameof(query)} cannot be null.");
            }

            if (query.Selector == null)
            {
                throw new ArgumentNullException(nameof(query.Selector), $"{nameof(query.Selector)} cannot be null.");
            }

            var queryable = ToQueryable(query);

            var entity = queryable.LastOrDefaultAsync(cancellationToken);

            return entity;
        }

        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var queryable = GetQueryable();

            var result = predicate == null ? queryable.AnyAsync(cancellationToken) : queryable.AnyAsync(predicate, cancellationToken);

            return result;
        }

        public virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var queryable = GetQueryable();

            var result = predicate == null ? queryable.CountAsync(cancellationToken) : queryable.CountAsync(predicate, cancellationToken);

            return result;
        }

        public virtual Task<long> LongCountAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            var queryable = GetQueryable();

            var result = predicate == null ? queryable.LongCountAsync(cancellationToken) : queryable.LongCountAsync(predicate, cancellationToken);

            return result;
        }

        public virtual Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector), $"{nameof(selector)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = predicate == null ? queryable.MaxAsync(selector, cancellationToken) : queryable.Where(predicate).MaxAsync(selector, cancellationToken);

            return result;
        }

        public virtual Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector), $"{nameof(selector)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = predicate == null ? queryable.MinAsync(selector, cancellationToken) : queryable.Where(predicate).MinAsync(selector, cancellationToken);

            return result;
        }

        public virtual Task<decimal> Average(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector), $"{nameof(selector)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = predicate == null ? queryable.AverageAsync(selector, cancellationToken) : queryable.Where(predicate).AverageAsync(selector, cancellationToken);

            return result;
        }

        public virtual Task<decimal> Sum(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector), $"{nameof(selector)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = predicate == null ? queryable.SumAsync(selector, cancellationToken) : queryable.Where(predicate).SumAsync(selector, cancellationToken);

            return result;
        }

        public virtual Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} cannot be null.");
            }

            var entityResult = DbSet.AddAsync(entity, cancellationToken).Then(result => result.Entity);

            return entityResult;
        }

        public virtual Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} cannot be null.");
            }

            if (!entities.Any())
            {
                return Task.CompletedTask;
            }

            return DbSet.AddRangeAsync(entities, cancellationToken);
        }

        public virtual Task<int> UpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> expression, CancellationToken cancellationToken = default)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), $"{nameof(predicate)} cannot be null.");
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), $"{nameof(expression)} cannot be null.");
            }

            var queryable = GetQueryable();

            var result = queryable.Where(predicate).UpdateAsync(expression, cancellationToken);

            return result;
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException($"{nameof(sql)} cannot be null or white-space.", nameof(sql));
            }

            var affectedRows = DbContext.Database.ExecuteSqlCommandAsync(sql, parameters ?? Enumerable.Empty<object>(), cancellationToken);

            return affectedRows;
        }

        #endregion IAsyncRepository<T> Members

        #region Private Methods

        private IQueryable<T> ToQueryable(IQuery<T> query)
        {
            IMultipleResultQuery<T> multipleResultQuery = null;

            if (query is IMultipleResultQuery<T>)
            {
                multipleResultQuery = (IMultipleResultQuery<T>)query;
            }

            var queryable = GetQueryable(query.QueryTrackingBehavior, query.IgnoreQueryFilters);

            if (query.Includes.Any())
            {
                queryable = queryable.Include(query.Includes);
            }

            if (query.Predicate != null)
            {
                queryable = queryable.Filter(query.Predicate);
            }

            if (query.Sortings.Any())
            {
                queryable = queryable.Sort(query.Sortings);
            }

            if (multipleResultQuery != null && multipleResultQuery.Topping.IsEnabled)
            {
                queryable = queryable.Top(multipleResultQuery.Topping);
            }

            if (multipleResultQuery != null && multipleResultQuery.Paging.IsEnabled)
            {
                var countQueryable = GetQueryable(multipleResultQuery.QueryTrackingBehavior, multipleResultQuery.IgnoreQueryFilters);

                if (multipleResultQuery.Includes.Any())
                {
                    countQueryable = countQueryable.Include(multipleResultQuery.Includes);
                }

                if (multipleResultQuery.Predicate != null)
                {
                    countQueryable = countQueryable.Filter(multipleResultQuery.Predicate);
                }

                multipleResultQuery.Paging.TotalCount = countQueryable.Count();

                queryable = queryable.Page(multipleResultQuery.Paging);
            }

            if (query.Selector != null)
            {
                queryable = queryable.Select(query.Selector);
            }

            return queryable;
        }

        private IQueryable<TResult> ToQueryable<TResult>(IQuery<T, TResult> query)
        {
            IMultipleResultQuery<T, TResult> multipleResultQuery = null;

            if (query is IMultipleResultQuery<T, TResult>)
            {
                multipleResultQuery = (IMultipleResultQuery<T, TResult>)query;
            }

            var queryable = GetQueryable(query.QueryTrackingBehavior, query.IgnoreQueryFilters);

            if (query.Includes.Any())
            {
                queryable = queryable.Include(query.Includes);
            }

            if (query.Predicate != null)
            {
                queryable = queryable.Filter(query.Predicate);
            }

            if (query.Sortings.Any())
            {
                queryable = queryable.Sort(query.Sortings);
            }

            if (multipleResultQuery != null && multipleResultQuery.Topping.IsEnabled)
            {
                queryable = queryable.Top(multipleResultQuery.Topping);
            }

            if (multipleResultQuery != null && multipleResultQuery.Paging.IsEnabled)
            {
                var countQueryable = GetQueryable(multipleResultQuery.QueryTrackingBehavior, multipleResultQuery.IgnoreQueryFilters);

                if (multipleResultQuery.Includes.Any())
                {
                    countQueryable = countQueryable.Include(multipleResultQuery.Includes);
                }

                if (multipleResultQuery.Predicate != null)
                {
                    countQueryable = countQueryable.Filter(multipleResultQuery.Predicate);
                }

                multipleResultQuery.Paging.TotalCount = countQueryable.Count();

                queryable = queryable.Page(multipleResultQuery.Paging);
            }

            return queryable.Select(query.Selector);
        }

        private IQueryable<T> GetQueryable(QueryTrackingBehavior? queryTrackingBehavior = null, bool? ignoreQueryFilters = null)
        {
            IQueryable<T> queryable = null;

            switch (queryTrackingBehavior ?? QueryTrackingBehavior.NoTracking)
            {
                case QueryTrackingBehavior.TrackAll:
                    {
                        queryable = DbSet.AsTracking();
                    }
                    break;
                case QueryTrackingBehavior.NoTracking:
                    {
                        queryable = DbSet.AsNoTracking();
                    }
                    break;
                default:
                    break;
            }

            if (ignoreQueryFilters ?? false)
            {
                queryable = queryable.IgnoreQueryFilters();
            }

            return queryable;
        }

        #endregion

        #region IDisposable Members

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}