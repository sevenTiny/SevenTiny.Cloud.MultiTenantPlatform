using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class MetaFieldRepository : IRepository<MetaField>
    {
        private readonly MultiTenantPlatformDbContext _context;
        public MetaFieldRepository(MultiTenantPlatformDbContext context)
        {
            _context = context;
        }

        public List<MetaField> GetList(Expression<Func<MetaField, bool>> filter)
        {
            return _context.QueryList<MetaField>(filter);
        }

        public MetaField GetEntity(Expression<Func<MetaField, bool>> filter)
        {
            return _context.QueryOne(filter);
        }

        public void Add(MetaField entity)
        {
            _context.Add(entity);
        }
        public void Add(IEnumerable<MetaField> entities)
        {
            _context.Add(entities);
        }

        public void Update(Expression<Func<MetaField, bool>> filter, MetaField entity)
        {
            _context.Update(filter, entity);
        }

        public void LogicDelete(Expression<Func<MetaField, bool>> filter, MetaField entity)
        {
            entity.IsDeleted = (int)IsDeleted.Deleted;
            Update(filter, entity);
        }

        public void Recover(Expression<Func<MetaField, bool>> filter, MetaField entity)
        {
            entity.IsDeleted = (int)IsDeleted.NotDeleted;
            Update(filter, entity);
        }

        public void Delete(Expression<Func<MetaField, bool>> filter)
        {
            _context.Delete(filter);
        }

        public bool Exist(Expression<Func<MetaField, bool>> filter)
        {
            return _context.QueryCount(filter) > 0;
        }
    }
}