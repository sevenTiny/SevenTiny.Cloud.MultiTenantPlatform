using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface IFieldListMetaFieldService : IRepository<FieldListMetaField>
    {
        Result<IList<FieldListMetaField>> Add(int metaObjectId, IList<FieldListMetaField> entities);
        FieldListMetaField GetById(int id);
        List<FieldListMetaField> GetByFieldListId(int interfaceFieldId);
        void DeleteByMetaFieldId(int metaFieldId);
        /// <summary>
        /// 获取接口配置的字段信息通过接口字段配置id
        /// </summary>
        /// <param name="interfaceFieldId"></param>
        /// <returns></returns>
        List<MetaField> GetMetaFieldsByFieldListId(int metaObjectId, int interfaceFieldId);
        /// <summary>
        /// 获取接口配置的字段信息通过接口字段配置id,返回字典形式
        /// </summary>
        /// <param name="interfaceFieldId"></param>
        /// <returns></returns>
        Dictionary<string, MetaField> GetMetaFieldsDicByFieldListId(int metaObjectId, int interfaceFieldId);
        List<Column> GetColumnDataByFieldListId(int interfaceFieldId);
        /// <summary>
        /// 字段排序
        /// </summary>
        /// <param name="interfaceFieldId">接口字段id</param>
        /// <param name="currentOrderIds">当前需要保存的元数据id顺序，这里定位具体数据id要配合接口字段id条件</param>
        void SortFields(int interfaceFieldId, int[] currentOrderIds);
    }
}
