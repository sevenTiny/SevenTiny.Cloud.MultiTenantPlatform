using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface IMenueService : ICommonInfoRepository<Menue>
    {
        List<Menue> GetUnDeletedEntitiesByApplicationId(int applicationId);
        Result<List<Menue>> AnalysisMenueTree();
    }
}
