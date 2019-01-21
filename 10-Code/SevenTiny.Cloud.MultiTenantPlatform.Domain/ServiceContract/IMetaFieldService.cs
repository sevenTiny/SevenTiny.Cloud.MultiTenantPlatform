using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IMetaFieldService : IMetaObjectManageRepository<MetaField>
    {
        /// <summary>
        /// 预置系统字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        void PresetFields(int metaObjectId);
    }
}
