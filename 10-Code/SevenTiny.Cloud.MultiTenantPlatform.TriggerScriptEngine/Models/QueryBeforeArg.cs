using MongoDB.Bson;
using MongoDB.Driver;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Models
{
    public class QueryBeforeArg
    {
        public string operateCode { get; set; }
        public FilterDefinition<BsonDocument> condition { get; set; }
    }
}
