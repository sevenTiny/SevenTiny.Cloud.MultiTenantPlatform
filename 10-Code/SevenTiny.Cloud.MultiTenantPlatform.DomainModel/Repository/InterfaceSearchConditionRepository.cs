using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class InterfaceSearchConditionRepository : IInterfaceSearchConditionRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public InterfaceSearchConditionRepository(MultiTenantPlatformDbContext context)
        {
            _context = context;
        }
        public void Add(InterfaceSearchCondition entity)
        {
            _context.Add(entity);
        }

        public void Add(IEnumerable<InterfaceSearchCondition> entities)
        {
            _context.Add(entities);
        }

        public void Delete(Expression<Func<InterfaceSearchCondition, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Expression<Func<InterfaceSearchCondition, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public InterfaceSearchCondition GetEntity(Expression<Func<InterfaceSearchCondition, bool>> filter)
        {
            return _context.QueryOne(filter);
        }

        public List<InterfaceSearchCondition> GetList(Expression<Func<InterfaceSearchCondition, bool>> filter)
        {
            return _context.QueryList(filter);
        }

        public void LogicDelete(Expression<Func<InterfaceSearchCondition, bool>> filter, InterfaceSearchCondition entity)
        {
            throw new NotImplementedException();
        }

        public void Recover(Expression<Func<InterfaceSearchCondition, bool>> filter, InterfaceSearchCondition entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Expression<Func<InterfaceSearchCondition, bool>> filter, InterfaceSearchCondition entity)
        {
            throw new NotImplementedException();
        }
    }
}
