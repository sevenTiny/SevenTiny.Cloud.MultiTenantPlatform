using SevenTiny.Bantina.Bankinate;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    public class MetaField : CommonInfo
    {
        [Column]
        public int MetaObjectId { get; set; }
        //=DataType
        [Column]
        public int FieldType { get; set; }
        //if field type is datasource
        [Column]
        public int DataSourceId { get; set; } = -1;
        [Column]
        public int IsMust { get; set; }
        [Column]
        public int IsSystem { get; set; } = (int)TrueFalse.False;
    }
}
