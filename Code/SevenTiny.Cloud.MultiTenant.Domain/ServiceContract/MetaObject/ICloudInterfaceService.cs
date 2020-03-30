using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface ICloudInterfaceService : IMetaObjectCommonServiceBase<CloudInterface>
    {
        /// <summary>
        /// 获取组织接口对象通过接口编码
        /// </summary>
        /// <param name="interfaceAggregationCode"></param>
        /// <returns></returns>
        CloudInterface GetByInterfaceAggregationCode(string interfaceAggregationCode);
        bool CheckFormIdExist(Guid formId);
    }
}
