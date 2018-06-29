using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
using System;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class ApplicationRepository : CommonRepository<Application>, IApplicationRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public ApplicationRepository(MultiTenantPlatformDbContext context) : base(context)
        {
            _context = context;
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
    }
}
