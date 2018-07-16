using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
using System;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class ConditionAggregationRepository : CommonRepository<ConditionAggregation>, IConditionAggregationRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public ConditionAggregationRepository(MultiTenantPlatformDbContext context) : base(context)
        {
            _context = context;
        }

        public ConditionAggregation GetConditionAggregationById(int id)
        {
            return GetEntity(t => t.Id == id);
        }

        public void LogicDelete(Expression<Func<ConditionAggregation, bool>> filter, ConditionAggregation entity)
        {
            throw new NotImplementedException();
        }

        public void Recover(Expression<Func<ConditionAggregation, bool>> filter, ConditionAggregation entity)
        {
            throw new NotImplementedException();
        }
    }
}
