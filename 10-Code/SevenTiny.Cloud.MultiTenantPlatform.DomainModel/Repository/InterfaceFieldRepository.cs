using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class InterfaceFieldRepository : IInterfaceFieldRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public InterfaceFieldRepository(MultiTenantPlatformDbContext context)
        {
            _context = context;
        }
        public void Add(InterfaceField entity)
        {
            _context.Add(entity);
        }

        public void Add(IEnumerable<InterfaceField> entities)
        {
            _context.Add(entities);
        }

        public void Delete(Expression<Func<InterfaceField, bool>> filter)
        {
            _context.Delete(filter);
        }

        public bool Exist(Expression<Func<InterfaceField, bool>> filter)
        {
            return _context.QueryCount(filter) > 0;
        }

        public InterfaceField GetEntity(Expression<Func<InterfaceField, bool>> filter)
        {
            return _context.QueryOne(filter);
        }

        public List<InterfaceField> GetList(Expression<Func<InterfaceField, bool>> filter)
        {
            return _context.QueryList(filter);
        }

        public void LogicDelete(Expression<Func<InterfaceField, bool>> filter, InterfaceField entity)
        {
            entity.IsDeleted = (int)IsDeleted.Deleted;
            Update(filter, entity);
        }

        public void Recover(Expression<Func<InterfaceField, bool>> filter, InterfaceField entity)
        {
            entity.IsDeleted = (int)IsDeleted.NotDeleted;
            Update(filter, entity);
        }

        public void Update(Expression<Func<InterfaceField, bool>> filter, InterfaceField entity)
        {
            _context.Update(filter, entity);
        }
    }
}
