using SevenTiny.Cloud.MultiTenantPlatform.CloudModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test.SevenTiny.Cloud.MultiTenantPlatform.UnitTest.CloudModel
{
   public class ObjectDataTest
    {
        [Fact]
        public void NewTest()
        {
            ObjectData obj = new ObjectData("wangdong3");

            obj["Name"] = 1;
            obj["Age"] = 1;
            obj["Sex"] = 1;

            Assert.NotNull(obj);
        }
    }
}
