using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    [Table]
    [TableCaching]
    public class MetaField : MetaObjectCommonBase
    {
        /// <summary>
        /// 不带对象编码的字段短编码
        /// </summary>
        [Column]
        public string ShortCode { get; set; }
        //=DataType
        [Column]
        public int FieldType { get; set; }
        //if field type is datasource
        [Column]
        public Guid DataSourceId { get; set; }
        [Column]
        public int IsSystem { get; set; } = (int)TrueFalse.False;
    }
}
