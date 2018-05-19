using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public interface IRepository<TEntity>
    {
        List<TEntity> GetList(Expression<Func<TEntity, bool>> filter);
        TEntity GetEntity(Expression<Func<TEntity, bool>> filter);
        bool Exist(Expression<Func<TEntity, bool>> filter);
        void Add(TEntity entity);
        void Update(Expression<Func<TEntity, bool>> filter,TEntity entity);
        void LogicDelete(Expression<Func<TEntity, bool>> filter, TEntity entity);
        void Recover(Expression<Func<TEntity, bool>> filter, TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> filter);
    }
}
