using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class ApplicationRepository : IRepository<Application>
    {
        private readonly MultiTenantPlatformDbContext _context;
        public ApplicationRepository(MultiTenantPlatformDbContext context)
        {
            _context = context;
        }

        public List<Application> GetList(Expression<Func<Application, bool>> filter)
        {
            return _context.QueryList(filter);
        }

        public Application GetEntity(Expression<Func<Application, bool>> filter)
        {
            return _context.QueryOne<Application>(filter);
        }

        public void Add(Application entity)
        {
            _context.Add(entity);
        }

        public void Update(Expression<Func<Application, bool>> filter, Application entity)
        {
            _context.Update(filter, entity);
        }

        public void LogicDelete(Expression<Func<Application, bool>> filter, Application entity)
        {
            entity.IsDeleted = (int)IsDeleted.Deleted;
            Update(filter, entity);
        }

        public void Recover(Expression<Func<Application, bool>> filter, Application entity)
        {
            entity.IsDeleted = (int)IsDeleted.NotDeleted;
            Update(filter, entity);
        }

        public void Delete(Expression<Func<Application, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Expression<Func<Application, bool>> filter)
        {
            return _context.QueryCount(filter) > 0;
        }
    }
}
