using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.UIMetaData
{
    /// <summary>
    /// button组件
    /// </summary>
    public class ButtonComponent
    {
        /// <summary>
        /// 按钮标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }
        /// <summary>
        /// 显示
        /// </summary>
        [JsonProperty("visible")]
        public bool Visible { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        [JsonProperty("disable")]
        public bool Disable { get; set; }
        /// <summary>
        /// 按钮说明
        /// </summary>
        [JsonProperty("tips")]
        public string Tips { get; set; }
        /// <summary>
        /// 动作类型
        /// </summary>
        [JsonProperty("action_type")]
        public int ActionType { get; set; }
        /// <summary>
        /// 动作说明
        /// </summary>
        [JsonProperty("action_description")]
        public string ActionDescription { get; set; }
        /// <summary>
        /// 按钮的参数
        /// </summary>
        [JsonProperty("params")]
        public string Params { get; set; }
    }
}
