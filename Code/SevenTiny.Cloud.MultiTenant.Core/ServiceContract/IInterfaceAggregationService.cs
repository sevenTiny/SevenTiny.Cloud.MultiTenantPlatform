using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Repository;

namespace SevenTiny.Cloud.MultiTenant.Core.ServiceContract
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
