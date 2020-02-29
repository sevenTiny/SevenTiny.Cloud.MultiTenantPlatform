using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MongoDB.Bson;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;

namespace Test.SevenTiny.Cloud.MultiTenant.Domain.BsonFormat
{
    public class BsonDocumentFormatTest
    {
        [Fact]
        public void JsonToBsonDocument()
        {
            string json = "{\"data\":[{\"name\":\"name\",\"value\":\"zhangsan\"},{\"name\":\"age\",\"value\":\"21\"}]}";
            var bson = BsonDocument.Parse(json);
        }

        [Fact]
        public void BsonToJson()
        {
            BsonDocument bson = new BsonDocument();
            bson.Add(new BsonElement("name", "zhangsan"));
            bson.Add(new BsonElement("age", "21"));

            BsonDocument child = new BsonDocument();
            child.Add(new BsonElement("data1", "1"));
            child.Add(new BsonElement("data2", "2"));

            bson.Add("child", child);

            var json = bson.ToJson();
        }
    }
}
