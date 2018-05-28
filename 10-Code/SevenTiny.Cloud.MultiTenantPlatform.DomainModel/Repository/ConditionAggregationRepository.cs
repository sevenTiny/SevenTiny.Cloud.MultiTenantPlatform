using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class ConditionAggregationRepository : IConditionAggregationRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public ConditionAggregationRepository(MultiTenantPlatformDbContext context)
        {
            _context = context;
        }
        public void Add(ConditionAggregation entity)
        {
            _context.Add(entity);
        }

        public void Add(IEnumerable<ConditionAggregation> entities)
        {
            _context.Add(entities);
        }

        public void Delete(Expression<Func<ConditionAggregation, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Expression<Func<ConditionAggregation, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public ConditionAggregation GetEntity(Expression<Func<ConditionAggregation, bool>> filter)
        {
            return _context.QueryOne(filter);
        }

        public List<ConditionAggregation> GetList(Expression<Func<ConditionAggregation, bool>> filter)
        {
            return _context.QueryList(filter);
        }

        public void LogicDelete(Expression<Func<ConditionAggregation, bool>> filter, ConditionAggregation entity)
        {
            throw new NotImplementedException();
        }

        public void Recover(Expression<Func<ConditionAggregation, bool>> filter, ConditionAggregation entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Expression<Func<ConditionAggregation, bool>> filter, ConditionAggregation entity)
        {
            throw new NotImplementedException();
        }
    }
}
