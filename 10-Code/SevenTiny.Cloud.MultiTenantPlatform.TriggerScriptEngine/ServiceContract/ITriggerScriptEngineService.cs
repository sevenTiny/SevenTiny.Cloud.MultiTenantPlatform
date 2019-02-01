using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.ServiceContract
{
    public interface ITriggerScriptEngineService
    {
        TableListComponent TableListAfter(int metaObjectId, string operateCode, TableListComponent tableListComponent);
        SingleObjectComponent SingleObjectAfter(int metaObjectId, string operateCode, SingleObjectComponent singleObjectComponent);
    }
}
