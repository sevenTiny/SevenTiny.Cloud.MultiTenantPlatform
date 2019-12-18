using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.Infrastructure.Const;
using System;

namespace SevenTiny.Cloud.Account.Core.Entity
{
    [Table]
    [TableCaching]
    public class Application
    {
        [Column]
        public string Icon { get; set; }
        [Key]
        [Column]
        [AutoIncrease]
        public int Id { get; set; }
        [Column("`Name`")]
        public string Name { get; set; }
        [Column("`Code`")]
        public string Code { get; set; }
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
