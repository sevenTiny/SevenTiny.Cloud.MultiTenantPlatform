using MongoDB.Bson;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface ITriggerScriptService : IMetaObjectCommonServiceBase<TriggerScript>
    {
        Result CompilateAndCheckScript(string script, string applicationCode);
        List<TriggerScript> GetTriggerScriptListUnDeletedByMetaObjectIdAndServiceType(Guid metaObjectId, int scriptType);
        string GetDefaultMetaObjectTriggerScriptByServiceTypeBefore(int serviceType);
        string GetDefaultMetaObjectTriggerScriptByServiceTypeAfter(int serviceType);
        string GetDefaultDataSourceTriggerScript();

        #region Execute Script
        T RunTriggerScript<T>(QueryPiplineContext queryPiplineContext, TriggerPoint triggerPoint, string functionName, T result, params object[] parameters);
        object RunDataSourceScript(QueryPiplineContext queryPiplineContext, params object[] parameters);
        #endregion
        List<TriggerScript> GetUnDeletedListByMetaObjectIdAndServiceType(Guid metaObjectId, int serviceType);
    }
}
