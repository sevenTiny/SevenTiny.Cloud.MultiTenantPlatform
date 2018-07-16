using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract
{
    public interface IConditionAggregationRepository : IRepository<ConditionAggregation>
    {
        ConditionAggregation GetConditionAggregationById(int id);
    }
}
