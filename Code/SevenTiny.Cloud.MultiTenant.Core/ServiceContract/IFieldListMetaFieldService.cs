﻿using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Repository;
using SevenTiny.Cloud.MultiTenant.Core.ValueObject;
using SevenTiny.Cloud.MultiTenant.UI.UIMetaData.ListView;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Core.ServiceContract
{
    public interface IFieldListMetaFieldService : IRepository<FieldListMetaField>
    {
        Result<IList<FieldListMetaField>> Add(int metaObjectId, IList<FieldListMetaField> entities);
        FieldListMetaField GetById(int id);
        List<FieldListMetaField> GetByFieldListId(int interfaceFieldId);
        void DeleteByMetaFieldId(int metaFieldId);
        List<Column> GetColumnDataByFieldListId(QueryPiplineContext queryPiplineContext);
        /// <summary>
        /// 字段排序
        /// </summary>
        /// <param name="interfaceFieldId">接口字段id</param>
        /// <param name="currentOrderIds">当前需要保存的元数据id顺序，这里定位具体数据id要配合接口字段id条件</param>
        void SortFields(int interfaceFieldId, int[] currentOrderIds);
    }
}