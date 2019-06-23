using SevenTiny.Bantina.Bankinate.Attributes;
using System;

namespace SevenTiny.Cloud.Account.Core.Entity
{
    /// <summary>
    /// EntityInfo Standard Property
    /// </summary>
    public abstract class CommonInfo
    {
        [Key]
        [Column]
        [AutoIncrease]
        public int Id { get; set; }
        [Column("`Description`")]
        public string Description { get; set; } = string.Empty;
        [Column("`Group`")]
        public string Group { get; set; } = string.Empty;
        [Column]
        public int SortNumber { get; set; } = 0;
        [Column]
        public int IsDeleted { get; set; } = 0;
        [Column]
        public int CreateBy { get; set; } = -1;
        [Column("`CreateTime`")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        [Column]
        public int ModifyBy { get; set; } = -1;
        [Column("`ModifyTime`")]
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
}
