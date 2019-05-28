using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    public class MetaField : MetaObjectManageInfo
    {
        //=DataType
        [Column]
        public int FieldType { get; set; }
        //if field type is datasource
        [Column]
        public int DataSourceId { get; set; } = -1;
        [Column]
        public int IsSystem { get; set; } = (int)TrueFalse.False;
    }
}
