using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
using System;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class InterfaceFieldRepository : CommonRepository<InterfaceField>, IInterfaceFieldRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public InterfaceFieldRepository(MultiTenantPlatformDbContext context) : base(context)
        {
            _context = context;
        }

        public void LogicDelete(Expression<Func<InterfaceField, bool>> filter, InterfaceField entity)
        {
            entity.IsDeleted = (int)IsDeleted.Deleted;
            Update(filter, entity);
        }

        public void Recover(Expression<Func<InterfaceField, bool>> filter, InterfaceField entity)
        {
            entity.IsDeleted = (int)IsDeleted.NotDeleted;
            Update(filter, entity);
        }
    }
}
