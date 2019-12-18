using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Core.ServiceContract
{
    public interface IMenueService : ICommonInfoRepository<Menue>
    {
        List<Menue> GetUnDeletedEntitiesByApplicationId(int applicationId);
        Result<List<Menue>> AnalysisMenueTree();
    }
}
