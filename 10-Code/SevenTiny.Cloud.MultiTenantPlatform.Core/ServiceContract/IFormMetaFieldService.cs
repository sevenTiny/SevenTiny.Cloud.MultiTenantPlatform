using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface IFormMetaFieldService : IRepository<FormMetaField>
    {
        FormMetaField GetById(int id);
        List<FormMetaField> GetByFormId(int formId);
        void DeleteByMetaFieldId(int metaFieldId);
        List<MetaField> GetMetaFieldsByFormId(int formId);
        Dictionary<string, MetaField> GetMetaFieldsDicByFormId(int formId);
        /// <summary>
        /// 字段排序
        /// </summary>
        /// <param name="formId">父对象id</param>
        /// <param name="currentOrderIds">当前需要保存的元数据id顺序，这里定位具体数据id要配合接口字段id条件</param>
        void SortFields(int formId, int[] currentOrderIds);
    }
}
