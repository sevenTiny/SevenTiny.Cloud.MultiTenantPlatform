using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Test.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Expression<Func<ConditionAggregation, bool>> func = t => t.Id == 1 && t.Name.Equals("3");

            Console.WriteLine(func.ToString());

            string test = "t => ((t.Id == 1) AndAlso t.Name.Equals(\"3\"))";

            var exp = JsonConvert.DeserializeObject<Expression<Func<ConditionAggregation, bool>>>(test);

            Console.WriteLine(JsonConvert.SerializeObject(func));


            //---------------
            Console.ReadKey();
        }
    }
}
