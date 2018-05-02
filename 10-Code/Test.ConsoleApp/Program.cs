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
                Application app = new Application();
                app.Code = "7TinyCloudDemo";
                app.Name = "7TinyCloud演示";

                db.Add(app);
            }

            //---------------
            Console.WriteLine("success!");
            Console.ReadKey();
        }
    }
}
