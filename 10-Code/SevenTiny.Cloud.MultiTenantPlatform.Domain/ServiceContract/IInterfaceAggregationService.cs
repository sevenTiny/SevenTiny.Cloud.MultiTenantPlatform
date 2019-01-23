using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IInterfaceAggregationService : IMetaObjectManageRepository<InterfaceAggregation>
    {
        /// <summary>
        /// 获取组织接口对象通过对象编码和接口编码
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="interfaceAggregationCode"></param>
        /// <returns></returns>
        InterfaceAggregation GetByMetaObjectIdAndInterfaceAggregationCode(int metaObjectId, string interfaceAggregationCode);
    }
}
