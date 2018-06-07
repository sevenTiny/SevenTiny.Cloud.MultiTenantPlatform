using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    /// <summary>
    /// Common Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CommonRepository<TEntity> where TEntity:class
    {
        private readonly MultiTenantPlatformDbContext _context;
        public CommonRepository(MultiTenantPlatformDbContext context)
        {
            _context = context;
        }
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
        {
            return _context.QueryList(filter);
        }

        public virtual TEntity GetEntity(Expression<Func<TEntity, bool>> filter)
        {
            return _context.QueryOne<TEntity>(filter);
        }
        public void Add(TEntity entity)
        {
            _context.Add(entity);
        }
        public void Add(IEnumerable<TEntity> entities)
        {
            _context.Add(entities);
        }

        public void Update(Expression<Func<TEntity, bool>> filter, TEntity entity)
        {
            _context.Update(filter, entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> filter)
        {
            _context.Delete(filter);
        }

        public bool Exist(Expression<Func<TEntity, bool>> filter)
        {
            return _context.QueryCount(filter) > 0;
        }
    }
}
