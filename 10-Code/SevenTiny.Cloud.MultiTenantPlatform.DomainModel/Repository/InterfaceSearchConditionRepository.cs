using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class InterfaceSearchConditionRepository : CommonRepository<InterfaceSearchCondition>, IInterfaceSearchConditionRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public InterfaceSearchConditionRepository(MultiTenantPlatformDbContext context) : base(context)
        {
            _context = context;
        }

        public void LogicDelete(Expression<Func<InterfaceSearchCondition, bool>> filter, InterfaceSearchCondition entity)
        {
            entity.IsDeleted = (int) IsDeleted.Deleted;
            _context.Update(filter, entity);
        }

        public void Recover(Expression<Func<InterfaceSearchCondition, bool>> filter, InterfaceSearchCondition entity)
        {
            entity.IsDeleted = (int)IsDeleted.NotDeleted;
            _context.Update(filter, entity);
        }
    }
}
