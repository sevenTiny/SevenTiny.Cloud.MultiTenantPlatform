using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System;

namespace Test.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var db = new MultiTenantPlatformDbContext())
            {
                var result = db.QueryList<Application>();
                Console.WriteLine(result.Count);
            }

            //---------------
            Console.ReadKey();
        }
    }
}
