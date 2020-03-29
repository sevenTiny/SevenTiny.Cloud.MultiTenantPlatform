using MongoDB.Bson;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IFormMetaFieldService : ICommonServiceBase<ConfigField>
    {
        Result<IList<ConfigField>> Add(int metaObjectId, IList<ConfigField> entities);
        ConfigField GetById(int id);
        List<ConfigField> GetByFormId(int formId);
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
        List<MetaField> GetSystemAndCustomListUnDeleted(Guid metaObjectId);
    }
}
