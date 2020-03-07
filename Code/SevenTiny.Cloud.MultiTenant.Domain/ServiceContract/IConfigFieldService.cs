using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IConfigFieldService : IMetaObjectCommonServiceBase<ConfigField>
    {
        //Result<IList<ListViewField>> Add(int metaObjectId, IList<ListViewField> entities);
        //ListViewField GetById(int id);
        //List<ListViewField> GetByFieldListId(int interfaceFieldId);
        //void DeleteByMetaFieldId(int metaFieldId);
        //List<Column> GetColumnDataByFieldListId(QueryPiplineContext queryPiplineContext);
        ///// <summary>
        ///// 字段排序
        ///// </summary>
        ///// <param name="interfaceFieldId">接口字段id</param>
        ///// <param name="currentOrderIds">当前需要保存的元数据id顺序，这里定位具体数据id要配合接口字段id条件</param>
        //void SortFields(int interfaceFieldId, int[] currentOrderIds);
    }
}
