using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Test.SevenTiny.Cloud.MultiTenantPlatform.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Expression<Func<ConditionAggregation, bool>> func = t => t.Id == 1 && t.Name.Equals("3");

            Console.WriteLine(func.ToString());

            string test = "t => ((t.Id == 1) AndAlso t.Name.Equals(\"3\"))";

            var exp = JsonConvert.DeserializeObject<Expression<Func<ConditionAggregation, bool>>>(test);

        }
    }
}
