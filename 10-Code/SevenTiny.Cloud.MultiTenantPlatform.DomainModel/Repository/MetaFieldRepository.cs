using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class MetaFieldRepository : CommonRepository<MetaField>, IMetaFieldRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public MetaFieldRepository(MultiTenantPlatformDbContext context) : base(context)
        {
            _context = context;
        }

        public void LogicDelete(Expression<Func<MetaField, bool>> filter, MetaField entity)
        {
            entity.IsDeleted = (int)IsDeleted.Deleted;
            Update(filter, entity);
        }

        public void Recover(Expression<Func<MetaField, bool>> filter, MetaField entity)
        {
            entity.IsDeleted = (int)IsDeleted.NotDeleted;
            Update(filter, entity);
        }
    }
}