using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject
{
    /// <summary>
    /// EntityInfo Standard Property
    /// </summary>
    public class CommonInfo
    {
        public Guid _id { get; set; } = Guid.NewGuid();
        public int IsDeleted { get; set; } = 0;
        public int CreateBy { get; set; } = -1;
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public int ModifyBy { get; set; } = -1;
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
}
