using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract
{
    public interface IInterfaceAggregationRepository : IRepository<InterfaceAggregation>
    {
        InterfaceAggregation GetInterfaceAggregationById(int id);
        InterfaceAggregation GetInterfaceAggregationByCode(string code);
    }
}
