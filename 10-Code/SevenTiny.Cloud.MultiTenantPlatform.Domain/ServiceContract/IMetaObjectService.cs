using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IMetaObjectService : IRepository<MetaObject>
    {
        List<MetaObject> GetMetaObjectsUnDeletedByApplicationId(int applicationId);
        List<MetaObject> GetMetaObjectsDeletedByApplicationId(int applicationId);
        MetaObject GetMetaObjectByCodeOrNameWithApplicationId(int applicationId, string code, string name);
        MetaObject GetMetaObjectByCodeAndApplicationId(int applicationId, string code);
        /// <summary>
        /// 检查是否存在相同名称但id不同的其他对象
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ExistSameNameWithOtherIdByApplicationId(int applicationId, int id, string name);
        /// <summary>
        /// 预置系统字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        void PresetFields(int metaObjectId);
    }
}
