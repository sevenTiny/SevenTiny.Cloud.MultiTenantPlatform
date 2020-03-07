using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject
{
    /// <summary>
    /// 为了提高查询性能，在该上下文中缓存查询管道中多次使用的数据
    /// </summary>
    public class QueryPiplineContext
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        public string InterfaceCode { get; set; }
        public Dictionary<string, object> ArgumentsDic { get; set; }

        public Guid ApplicationId { get; set; }
        public string ApplicationCode { get; set; }
        public CloudApplication Application { get; set; }

        public Guid MetaObjectId { get; set; }
        public string MetaObjectCode { get; set; }
        public MetaObject MetaObject { get; set; }

        public Guid SearchConditionId { get; set; }
        public Guid FieldListId { get; set; }
        public Guid FormId { get; set; }
        public Guid DataSourceId { get; set; }
        public InterfaceType InterfaceType { get; set; }

        private List<MetaField> _MetaFieldsUnDeleted;
        /// <summary>
        /// 对象下的全部字段的集合
        /// </summary>
        public List<MetaField> MetaFieldsUnDeleted
        {
            get { return _MetaFieldsUnDeleted; }
            set
            {
                _MetaFieldsUnDeleted = value;
                MetaFieldsUnDeletedIdDic = _MetaFieldsUnDeleted.ToDictionary(t => t.Id, t => t);
                MetaFieldsUnDeletedUpperCodeDic = _MetaFieldsUnDeleted.ToDictionary(t => t.Code.ToUpperInvariant(), t => t);
                MetaFieldsUnDeletedCodeDic = _MetaFieldsUnDeleted.ToDictionary(t => t.Code, t => t);
            }
        }
        /// <summary>
        /// 对象下全部字段转化成以id为key的字典
        /// </summary>
        public Dictionary<Guid, MetaField> MetaFieldsUnDeletedIdDic { get; private set; }
        /// <summary>
        /// 对象下的全部字段转化为以大写的code为key的字典
        /// </summary>
        public Dictionary<string, MetaField> MetaFieldsUnDeletedUpperCodeDic { get; private set; }
        /// <summary>
        /// 对象下的全部字段转化为以code为key的字典
        /// </summary>
        public Dictionary<string, MetaField> MetaFieldsUnDeletedCodeDic { get; private set; }
        /// <summary>
        /// 某个服务类型下的全部触发器脚本集合
        /// </summary>
        public List<TriggerScript> TriggerScriptsOfOneServiceType { get; set; }

        /// <summary>
        /// 当前FieldListId对应的全部FieldListMetaFields
        /// </summary>
        public List<ConfigField> FieldListMetaFieldsOfFieldListId { get; set; }
        /// <summary>
        /// 当前FieldListMetaFieldsOfFieldListId映射的MetaFields
        /// </summary>
        public List<MetaField> MetaFieldsOfFieldListId
        {
            get
            {
                if (FieldListMetaFieldsOfFieldListId != null && FieldListMetaFieldsOfFieldListId.Any())
                {
                    var fieldIds = FieldListMetaFieldsOfFieldListId.Select(t => t.MetaFieldId).ToArray();
                    return MetaFieldsUnDeleted.Where(t => fieldIds.Contains(t.Id)).ToList();
                }
                return null;
            }
        }
        /// <summary>
        /// 当前FieldListMetaFieldsOfFieldListId映射的MetaFields的以Code为key的字典形式
        /// </summary>
        public Dictionary<string, MetaField> MetaFieldsCodeDicByFieldListId
        {
            get
            {
                return MetaFieldsOfFieldListId?.ToDictionary(t => t.Code, t => t);
            }
        }
    }
}
