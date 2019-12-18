using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Enum;
using SevenTiny.Cloud.MultiTenant.Core.Repository;
using SevenTiny.Cloud.MultiTenant.Core.ValueObject;
using SevenTiny.Cloud.MultiTenant.UI.UIMetaData.ListView;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Core.ServiceContract
{
    public interface ITriggerScriptService : IMetaObjectManageRepository<TriggerScript>
    {
        Result CompilateAndCheckScript(string script, string applicationCode);
        List<TriggerScript> GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(int metaObjectId, int scriptType);
        string GetDefaultMetaObjectTriggerScriptByServiceTypeBefore(int serviceType);
        string GetDefaultMetaObjectTriggerScriptByServiceTypeAfter(int serviceType);
        string GetDefaultDataSourceTriggerScript();

        #region Execute Script
        T RunTriggerScript<T>(QueryPiplineContext queryPiplineContext, TriggerPoint triggerPoint, string functionName, T result, params object[] parameters);
        object RunDataSourceScript(QueryPiplineContext queryPiplineContext, params object[] parameters);
        #endregion
    }
}
