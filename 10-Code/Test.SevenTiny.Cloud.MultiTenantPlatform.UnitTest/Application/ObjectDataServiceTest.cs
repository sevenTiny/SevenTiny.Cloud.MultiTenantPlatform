using SevenTiny.Cloud.MultiTenantPlatform.Application.Service;
using SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.CloudModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test.SevenTiny.Cloud.MultiTenantPlatform.UnitTest.Application
{
    public class ObjectDataServiceTest
    {
        [Fact]
        public void InsertTest()
        {
            IObjectDataService s = new ObjectDataService();

            ObjectData obj = new ObjectData("wangdong3");
            obj["Name"] = 3;
            obj["Age"] = 3;
            obj["Sex"] = 3;

            s.Insert(obj);
        }
    }
}
