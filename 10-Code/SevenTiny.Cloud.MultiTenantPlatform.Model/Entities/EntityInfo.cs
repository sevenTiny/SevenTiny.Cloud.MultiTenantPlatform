using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Model.Entities
{
    public class EntityInfo
    {
        public string Group { get; set; }
        public int SortNumber { get; set; }
        public int IsDeleted { get; set; }
        public int CreateBy { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public int ModifyBy { get; set; }
        public DateTimeOffset ModifyTime { get; set; }

    }
}
