using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Models
{
    public class BatchAddArg
    {
        public string operateCode { get; set; }
        public List<MongoDB.Bson.BsonDocument> bsonElementsList { get; set; }
    }
}
