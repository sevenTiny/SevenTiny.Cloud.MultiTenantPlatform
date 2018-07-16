using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
using System;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class InterfaceAggregationRepository : CommonRepository<InterfaceAggregation>, IInterfaceAggregationRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public InterfaceAggregationRepository(MultiTenantPlatformDbContext context) : base(context)
        {
            _context = context;
        }

        public InterfaceAggregation GetInterfaceAggregationByCode(string code)
        {
            return GetEntity(t => t.Code.Equals(code));
        }

        public InterfaceAggregation GetInterfaceAggregationById(int id)
        {
            return GetEntity(t => t.Id == id);
        }

        public void LogicDelete(Expression<Func<InterfaceAggregation, bool>> filter, InterfaceAggregation entity)
        {
            entity.IsDeleted = (int)IsDeleted.Deleted;
            _context.Update(filter, entity);
        }

        public void Recover(Expression<Func<InterfaceAggregation, bool>> filter, InterfaceAggregation entity)
        {
            entity.IsDeleted = (int)IsDeleted.NotDeleted;
            _context.Update(filter, entity);
        }
    }
}
