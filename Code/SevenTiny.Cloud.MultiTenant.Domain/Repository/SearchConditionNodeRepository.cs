using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    internal class SearchConditionNodeRepository : CommonRepositoryBase<SearchConditionNode>, ISearchConditionNodeRepository
    {
        public SearchConditionNodeRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
        }

        public List<SearchConditionNode> GetListBySearchConditionId(Guid id)
            => _dbContext.SearchConditionNode.Where(t => t.SearchConditionId == id).ToList();

        public List<SearchConditionNode> GetParameterTypeListBySearchConditionId(Guid searchConditionId)
            => _dbContext.SearchConditionNode.Where(t => t.SearchConditionId == searchConditionId && t.MetaFieldId != Guid.Empty && t.ValueType == (int)ConditionValueType.Parameter).ToList();
    }
}
