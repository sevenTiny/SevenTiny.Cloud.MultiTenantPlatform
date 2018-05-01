using SevenTiny.Cloud.MultiTenantPlatform.Model.Enums;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    public class MetaField: EntityInfo
    {
        public int EntityId { get; set; }
        public FieldType FieldType { get; set; }
        public int IsMust { get; set; }
        public int IsSystem { get; set; }
    }
}
