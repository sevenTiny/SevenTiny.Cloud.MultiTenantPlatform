using MongoDB.Bson;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Core.ServiceContract
{
    public interface IFormMetaFieldService : IRepository<FormMetaField>
    {
        Result<IList<FormMetaField>> Add(int metaObjectId, IList<FormMetaField> entities);
        FormMetaField GetById(int id);
        List<FormMetaField> GetByFormId(int formId);
        void DeleteByMetaFieldId(int metaFieldId);
        List<MetaField> GetMetaFieldsByFormId(int metaObjectId, int formId);
        /// <summary>
        /// 字段排序
        /// </summary>
        /// <param name="formId">父对象id</param>
        /// <param name="currentOrderIds">当前需要保存的元数据id顺序，这里定位具体数据id要配合接口字段id条件</param>
        void SortFields(int formId, int[] currentOrderIds);
        /// <summary>
        /// 通过Form配置进行数据校验
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="bsonElements"></param>
        /// <returns></returns>
        Result ValidateFormData(int formId, BsonDocument bsonElements);
        Result ValidateFormData(int formId, List<BsonDocument> bsonElements);
    }
}
