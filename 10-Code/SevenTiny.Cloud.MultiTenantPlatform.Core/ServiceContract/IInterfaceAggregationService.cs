using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface IInterfaceAggregationService : IMetaObjectManageRepository<InterfaceAggregation>
    {
        /// <summary>
        /// 获取组织接口对象通过接口编码
        /// </summary>
        /// <param name="interfaceAggregationCode"></param>
        /// <returns></returns>
        InterfaceAggregation GetByInterfaceAggregationCode(string interfaceAggregationCode);
    }
}
