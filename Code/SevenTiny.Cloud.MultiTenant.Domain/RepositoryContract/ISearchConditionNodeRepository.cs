using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    public interface ISearchConditionNodeRepository : ICommonRepositoryBase<SearchConditionNode>
    {
        List<SearchConditionNode> GetListBySearchConditionId(Guid id);
        List<SearchConditionNode> GetParameterTypeListBySearchConditionId(Guid id);
    }
}
