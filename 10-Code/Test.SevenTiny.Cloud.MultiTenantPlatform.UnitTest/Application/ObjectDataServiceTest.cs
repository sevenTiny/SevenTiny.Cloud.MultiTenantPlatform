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

            obj["Name"] = 111;
            obj["Age"] = 11;
            obj["Sex"] = 1;

            s.Insert(obj);
        }
    }
}
