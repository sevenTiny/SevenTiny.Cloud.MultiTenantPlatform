using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
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
