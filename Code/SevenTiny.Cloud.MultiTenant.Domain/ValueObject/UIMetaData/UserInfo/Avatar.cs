using Newtonsoft.Json;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.UserInfo
{
    /// <summary>
    /// 头像信息
    /// </summary>
    public class Avatar
    {
        [JsonProperty("hasAvatar", NullValueHandling = NullValueHandling.Ignore)]
        public bool HasAvatar { get; set; }
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }
        [JsonProperty("original", NullValueHandling = NullValueHandling.Ignore)]
        public string Original { get; set; }
        [JsonProperty("big", NullValueHandling = NullValueHandling.Ignore)]
        public string Big { get; set; }
        [JsonProperty("medium", NullValueHandling = NullValueHandling.Ignore)]
        public string Medium { get; set; }
        [JsonProperty("small", NullValueHandling = NullValueHandling.Ignore)]
        public string Small { get; set; }
    }
}
