using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface ITriggerScriptService : IMetaObjectManageRepository<TriggerScript>
    {
        Result CompilateAndCheckScript(string script, string applicationCode);
        List<TriggerScript> GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(int metaObjectId, int scriptType);
        string GetDefaultMetaObjectTriggerScriptByServiceTypeBefore(int serviceType);
        string GetDefaultMetaObjectTriggerScriptByServiceTypeAfter(int serviceType);
        string GetDefaultDataSourceTriggerScript();

        #region Execute Script
        T RunTriggerScript<T>(int metaObjectId, string applicationCode, ServiceType serviceType, TriggerPoint triggerPoint, string functionName, ref T result, params object[] parameters);
        object RunDataSourceScript(string applicationCode, int dataSourceId, params object[] parameters);
        #endregion
    }
}
