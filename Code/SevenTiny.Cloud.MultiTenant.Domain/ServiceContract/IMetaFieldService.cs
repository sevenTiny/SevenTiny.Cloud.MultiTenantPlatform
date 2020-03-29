using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IMetaFieldService : IMetaObjectCommonServiceBase<MetaField>
    {
        /// <summary>
        /// 获取字段的字典形式
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        Dictionary<string, MetaField> GetMetaFieldDicUnDeleted(Guid metaObjectId);
        /// <summary>
        /// 获取字段的字典形式，key是大写的
        /// ToUpperInvariant方法是微软优化过的，性能要高很多
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        Dictionary<string, MetaField> GetMetaFieldUpperKeyDicUnDeleted(Guid metaObjectId);
        /// <summary>
        /// 获取字段的字典形式，key是Id
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <returns></returns>
        Dictionary<Guid, MetaField> GetMetaFieldIdDicUnDeleted(Guid metaObjectId);
        /// <summary>
        /// 预置系统字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        void PresetFields(Guid metaObjectId);
        /// <summary>
        /// 获取预置字段的字典形式
        /// </summary>
        /// <returns></returns>
        BsonDocument GetPresetFieldBsonElements();
        /// <summary>
        /// 校验字段传入的值是否符合类型
        /// 如果符合类型则返回true并且将value转化成符合条件的类型
        /// 如果不符合，则返回false
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Result<dynamic> CheckAndGetFieldValueByFieldType(Guid fieldId, object value);
        Result<dynamic> CheckAndGetFieldValueByFieldType(MetaField metaField, object value);
        /// <summary>
        /// 获取字段列表集合通过字段id集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<MetaField> GetByIds(Guid metaObjectId, Guid[] ids);
        /// <summary>
        /// 生成mongodb的排序字段配置
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="sortFields"></param>
        /// <returns></returns>
        SortDefinition<BsonDocument> GetSortDefinitionBySortFields(QueryPiplineContext queryPiplineContext, SortField[] sortFields);
    }
}
