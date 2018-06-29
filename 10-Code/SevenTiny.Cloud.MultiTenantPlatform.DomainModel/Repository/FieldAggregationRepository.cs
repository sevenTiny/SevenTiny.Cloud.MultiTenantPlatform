using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
using System;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class FieldAggregationRepository : CommonRepository<FieldAggregation>, IFieldAggregationRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public FieldAggregationRepository(MultiTenantPlatformDbContext context) : base(context)
        {
            _context = context;
        }

        public void LogicDelete(Expression<Func<FieldAggregation, bool>> filter, FieldAggregation entity)
        {
            throw new NotImplementedException();
        }

        public void Recover(Expression<Func<FieldAggregation, bool>> filter, FieldAggregation entity)
        {
            throw new NotImplementedException();
        }
    }
}
