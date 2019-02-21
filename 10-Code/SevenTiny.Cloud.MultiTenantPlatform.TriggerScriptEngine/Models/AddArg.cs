namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Models
{
    public class AddArg
    {
        public string operateCode { get; set; }
        public MongoDB.Bson.BsonDocument bsonElements { get; set; }
    }
}
