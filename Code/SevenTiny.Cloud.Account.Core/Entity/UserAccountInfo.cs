using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.Account.Core.Entity
{
    [Table]
    public class UserAccountInfo : CommonInfo
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column]
        public int UserId { get; set; }
    }
}
