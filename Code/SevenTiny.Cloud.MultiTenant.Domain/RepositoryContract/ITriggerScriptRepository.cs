using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    internal interface ITriggerScriptRepository : IMetaObjectCommonRepositoryBase<TriggerScript>
    {
        List<TriggerScript> GetUnDeletedListByMetaObjectIdAndServiceType(Guid metaObjectId, int serviceType);
    }
}
