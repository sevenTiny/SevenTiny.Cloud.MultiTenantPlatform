using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IMetaFieldService : IMetaObjectManageRepository<MetaField>
    {
        /// <summary>
        /// 预置系统字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        void PresetFields(int metaObjectId);
        /// <summary>
        /// 获取预置字段的字典形式
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetPresetFieldDic();
    }
}
