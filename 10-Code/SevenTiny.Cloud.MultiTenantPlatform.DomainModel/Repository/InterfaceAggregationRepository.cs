using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class InterfaceAggregationRepository : IInterfaceAggregationRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public InterfaceAggregationRepository(MultiTenantPlatformDbContext context)
        {
            _context = context;
        }
        public void Add(InterfaceAggregation entity)
        {
            _context.Add(entity);
        }

        public void Add(IEnumerable<InterfaceAggregation> entities)
        {
            _context.Add(entities);
        }

        public void Delete(Expression<Func<InterfaceAggregation, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Expression<Func<InterfaceAggregation, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public InterfaceAggregation GetEntity(Expression<Func<InterfaceAggregation, bool>> filter)
        {
            return _context.QueryOne(filter);
        }

        public List<InterfaceAggregation> GetList(Expression<Func<InterfaceAggregation, bool>> filter)
        {
            return _context.QueryList(filter);
        }

        public void LogicDelete(Expression<Func<InterfaceAggregation, bool>> filter, InterfaceAggregation entity)
        {
            throw new NotImplementedException();
        }

        public void Recover(Expression<Func<InterfaceAggregation, bool>> filter, InterfaceAggregation entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Expression<Func<InterfaceAggregation, bool>> filter, InterfaceAggregation entity)
        {
            throw new NotImplementedException();
        }
    }
}
