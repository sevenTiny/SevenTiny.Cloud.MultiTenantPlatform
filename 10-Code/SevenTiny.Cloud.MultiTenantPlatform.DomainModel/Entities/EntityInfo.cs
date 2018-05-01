using SevenTiny.Bantina.Bankinate;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    /// <summary>
    /// EntityInfo Standard Property
    /// </summary>
    public class EntityInfo
    {
        [Key]
        [AutoIncrease]
        public int Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string Code { get; set; }
        [Column]
        public string Description { get; set; }
        [Column]
        public string Group { get; set; }
        [Column]
        public int SortNumber { get; set; }
        [Column]
        public int IsDeleted { get; set; } = 0;
        [Column]
        public int CreateBy { get; set; }
        [Column]
        public DateTimeOffset CreateTime { get; set; }
        [Column]
        public int ModifyBy { get; set; }
        [Column]
        public DateTimeOffset ModifyTime { get; set; }
    }
}
