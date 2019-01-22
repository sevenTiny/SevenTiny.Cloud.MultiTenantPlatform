using MongoDB.Bson;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IMetaFieldService : IMetaObjectManageRepository<MetaField>
    {
        /// <summary>
        /// 获取字段的字典形式
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        Dictionary<string, MetaField> GetMetaFieldDicUnDeleted(int metaObjectId);
        /// <summary>
        /// 获取字段的字典形式，key是小写的
        /// ToUpperInvariant方法是微软优化过的，性能要高很多
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        Dictionary<string, MetaField> GetMetaFieldUpperKeyDicUnDeleted(int metaObjectId);
        /// <summary>
        /// 预置系统字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        void PresetFields(int metaObjectId);
        /// <summary>
        /// 获取预置字段的字典形式
        /// </summary>
        /// <returns></returns>
        BsonDocument GetPresetFieldBsonElements();
    }
}
