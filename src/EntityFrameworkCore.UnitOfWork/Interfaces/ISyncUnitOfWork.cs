﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;

namespace EntityFrameworkCore.UnitOfWork.Interfaces
{
    public interface ISyncUnitOfWork : IRepositoryFactory, IDisposable
    {
        TimeSpan? Timeout { get; set; }
        bool HasTransaction();
        bool HasChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess = true);
        void DiscardChanges();
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        int ExecuteSqlCommand(string sql, params object[] parameters);
        IList<T> FromSql<T>(string sql, params object[] parameters) where T : class;
        void ChangeDatabase(string database);
    }

    public interface ISyncUnitOfWork<T> : ISyncUnitOfWork, IRepositoryFactory<T>, IDisposable where T : DbContext
    { }
}