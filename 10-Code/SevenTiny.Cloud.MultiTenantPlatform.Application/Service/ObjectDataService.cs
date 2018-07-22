using MongoDB.Bson;
using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.CloudModel;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application.Service
{
    public class ObjectDataService : IObjectDataService
    {
        public void Insert(ObjectData objectData)
        {
            using (var fact = new MultiTenantDataDbContext())
            {
                //todu : 强类型对象转bson 的方法
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectData);
                BsonDocument bson = JsonConvert.DeserializeObject<BsonDocument>(json);
                fact.Add<ObjectData>(bson);
            }
        }

        public void Update(ObjectData objectData)
        {
            throw new NotImplementedException();
        }
    }
}
