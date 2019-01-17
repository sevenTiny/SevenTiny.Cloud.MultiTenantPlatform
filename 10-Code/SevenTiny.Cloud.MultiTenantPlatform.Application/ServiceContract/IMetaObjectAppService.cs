using SevenTiny.Cloud.MultiTenantPlatform.Application.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract
{
    public interface IMetaObjectAppService
    {
        /// <summary>
        /// 添加对象，并对参数进行校验
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="applicationCode"></param>
        /// <param name="metaObject"></param>
        /// <returns></returns>
        ResultModel AddMetaObject(int applicationId,string applicationCode, MetaObject metaObject);
        /// <summary>
        /// 删除对象数据，并将对象下面的字段全部清空（事务操作）
        /// </summary>
        /// <param name="metaObjectId"></param>
        void Delete(int metaObjectId);
    }
}
