using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Model.Entities
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public int SortNumber { get; set; }
        public int IsDeleted { get; set; }
        public int CreateBy { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public int ModifyBy { get; set; }
        public DateTimeOffset ModifyTime { get; set; }
    }
}
