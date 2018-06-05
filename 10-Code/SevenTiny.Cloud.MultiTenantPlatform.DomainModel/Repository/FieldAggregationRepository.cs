using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class FieldAggregationRepository : IFieldAggregationRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public FieldAggregationRepository(MultiTenantPlatformDbContext context)
        {
            _context = context;
        }
        public void Add(FieldAggregation entity)
        {
            _context.Add(entity);
        }

        public void Add(IEnumerable<FieldAggregation> entities)
        {
            _context.Add(entities);
        }

        public void Delete(Expression<Func<FieldAggregation, bool>> filter)
        {
            _context.Delete(filter);
        }

        public bool Exist(Expression<Func<FieldAggregation, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public FieldAggregation GetEntity(Expression<Func<FieldAggregation, bool>> filter)
        {
            return _context.QueryOne(filter);
        }

        public List<FieldAggregation> GetList(Expression<Func<FieldAggregation, bool>> filter)
        {
            return _context.QueryList(filter);
        }

        public void LogicDelete(Expression<Func<FieldAggregation, bool>> filter, FieldAggregation entity)
        {
            throw new NotImplementedException();
        }

        public void Recover(Expression<Func<FieldAggregation, bool>> filter, FieldAggregation entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Expression<Func<FieldAggregation, bool>> filter, FieldAggregation entity)
        {
            throw new NotImplementedException();
        }
    }
}
