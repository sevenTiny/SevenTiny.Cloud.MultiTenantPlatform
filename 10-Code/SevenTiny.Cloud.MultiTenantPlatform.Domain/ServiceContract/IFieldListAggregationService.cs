using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.UIMetaData.ListView;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IFieldListAggregationService : IRepository<FieldListAggregation>
    {
        List<FieldListAggregation> GetByFieldListId(int interfaceFieldId);
        void DeleteByMetaFieldId(int metaFieldId);
        /// <summary>
        /// 获取接口配置的字段信息通过接口字段配置id
        /// </summary>
        /// <param name="interfaceFieldId"></param>
        /// <returns></returns>
        List<MetaField> GetMetaFieldsByFieldListId(int interfaceFieldId);
        /// <summary>
        /// 获取接口配置的字段信息通过接口字段配置id,返回字典形式
        /// </summary>
        /// <param name="interfaceFieldId"></param>
        /// <returns></returns>
        Dictionary<string, MetaField> GetMetaFieldsDicByFieldListId(int interfaceFieldId);
        List<Column> GetColumnDataByFieldListId(int interfaceFieldId);
    }
}
