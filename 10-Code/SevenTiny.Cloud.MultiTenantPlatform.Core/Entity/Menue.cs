using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    [Table]
    [TableCaching]
    public class Menue : CommonInfo
    {
        [Column]
        public int ApplicationId { get; set; }
        /// <summary>
        /// Not Column
        /// </summary>
        public string ApplicationCode { get; set; }

        [Column("Icon")]
        public string Icon { get; set; }
        [Column]
        public int LinkType { get; set; }
        [Column]
        public string Address { get; set; }

        public List<Menue> Children { get; set; }
    }
}
