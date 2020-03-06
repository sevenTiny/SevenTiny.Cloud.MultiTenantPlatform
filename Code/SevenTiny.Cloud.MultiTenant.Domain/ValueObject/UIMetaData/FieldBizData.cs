using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.UserInfo;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData
{
    /// <summary>
    /// 业务字段
    /// </summary>
    public class FieldBizData
    {
        //
        // Summary:
        //     排序号
        [JsonProperty("num", Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public string Num { get; set; }
        //
        // Summary:
        //     字段名
        [JsonProperty("name", Order = 1)]
        public string Name { get; set; }
        //
        // Summary:
        //     显示文本
        [JsonProperty("text", Order = 2)]
        public string Text { get; set; }
        //
        // Summary:
        //     存储值
        [JsonProperty("value", Order = 3)]
        public object Value { get; set; }
        //
        // Summary:
        //     存储值

        [JsonProperty("clientUrl", Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public string ClientUrl { get; set; }
        //
        // Summary:
        //     存储值
        [JsonProperty("downloadUrl", Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public string DownloadUrl { get; set; }
        //
        // Summary:
        //     用户头像信息
        [JsonProperty("avatars", Order = 3, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Avatar> Avatars { get; set; }
    }
}
