using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface ITriggerScriptService : IMetaObjectManageRepository<TriggerScript>
    {
        TableListComponent TableListAfter(TableListComponent tableListComponent, int triggerScriptId);
    }
}
