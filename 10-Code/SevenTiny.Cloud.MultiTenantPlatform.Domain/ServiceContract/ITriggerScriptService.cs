using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface ITriggerScriptService : IMetaObjectManageRepository<TriggerScript>
    {
        List<TriggerScript> GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(int metaObjectId, int scriptType, int triggerPoint);
        string GetDefaultTriggerScriptByScriptTypeAndTriggerPoint(int scriptType, int triggerPoint);
    }
}
