using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface IMetaObjectService : ICommonInfoRepository<MetaObject>
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
        /// 添加对象，并对参数进行校验
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationCode"></param>
        /// <param name="metaObject"></param>
        /// <returns></returns>
        Result AddMetaObject(int applicationId, string applicationCode, MetaObject metaObject);
    }
}
