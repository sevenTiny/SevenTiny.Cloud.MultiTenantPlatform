using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface ITriggerScriptService : IMetaObjectManageRepository<TriggerScript>
    {
        List<TriggerScript> GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(int metaObjectId, int scriptType);
        string GetDefaultTriggerScriptByScriptType(int scriptType);
        string GetDefaultTriggerScriptDataSourceScript();
    }
}
