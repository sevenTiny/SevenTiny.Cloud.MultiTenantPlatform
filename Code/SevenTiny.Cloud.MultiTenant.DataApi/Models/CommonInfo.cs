using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    /// <summary>
    /// EntityInfo Standard Property
    /// </summary>
    public class CommonInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public int SortNumber { get; set; } = 0;
        public int IsDeleted { get; set; } = 0;
        public int CreateBy { get; set; } = -1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int ModifyBy { get; set; } = -1;
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
}
