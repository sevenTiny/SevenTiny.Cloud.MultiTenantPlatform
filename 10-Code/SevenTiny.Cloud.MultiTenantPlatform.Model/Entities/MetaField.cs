using SevenTiny.Cloud.MultiTenantPlatform.Model.Enums;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Model.Entities
{
    public class MetaField: EntityInfo
    {
        public int EntityId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public FieldType FieldType { get; set; }
        public int IsMust { get; set; }
        public int IsSystem { get; set; }
        public int SortNumber { get; set; }
        public int IsDeleted { get; set; }
        public int CreateBy { get; set; }
        public DateTimeOffset CreateTime { get; set; }
    }
}
