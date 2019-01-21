using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IConditionAggregationService : IRepository<ConditionAggregation>
    {
        List<ConditionAggregation> GetByInterfaceConditionId(int interfaceSearchConditionId);
        ResultModel AggregateCondition(int interfaceConditionId, int brotherNodeId, int conditionJointTypeId, int fieldId, int conditionTypeId, string conditionValue, int conditionValueTypeId);

        ResultModel DeleteAggregateCondition(int nodeId, int interfaceSearchConditionId);
    }
}
