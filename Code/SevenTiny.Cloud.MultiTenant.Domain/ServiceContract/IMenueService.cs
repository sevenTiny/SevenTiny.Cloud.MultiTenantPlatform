using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IMenueService : ICommonInfoRepository<Menue>
    {
        List<Menue> GetUnDeletedEntitiesByApplicationId(int applicationId);
        Result<List<Menue>> AnalysisMenueTree();
    }
}
