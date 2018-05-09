using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System;

namespace Test.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            //using (var db = new MultiTenantPlatformDbContext())
            //{
            //    Application app = new Application();
            //    app.Code = "7TinyCloudDemo";
            //    app.Name = "7TinyCloud演示";

            //    db.Add(app);
            //}


            int n = 5;
            int max = 0;

            for (int i = 0; i < n; i++)
            {
                //输入一个数
                int input = Convert.ToInt32(Console.ReadLine());
                if (input >= 100 && input <= 200 && input > max)
                {
                    max = input;
                }
            }

            Console.WriteLine(max);


            //---------------
            Console.WriteLine("success!");
            Console.ReadKey();
        }
    }
}
