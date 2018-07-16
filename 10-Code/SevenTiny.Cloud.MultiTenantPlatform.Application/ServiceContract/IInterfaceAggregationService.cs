using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract
{
    public interface IInterfaceAggregationService
    {
        
        ConditionAggregation GetConditionAggregationByInterfaceAggregationId(int interfaceAggregationId);
        ConditionAggregation GetConditionAggregationByInterfaceAggregationCode(string interfaceAggregationCode);
    }
}
