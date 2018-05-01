using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class ApplicationRepository
    {
        public static List<Application> GetApplicationList()
        {
            using (var db = new MultiTenantPlatformDbContext())
            {
                return db.QueryList<Application>();
            }
        }
        public static void AddApplication(Application application)
        {
            using (var db = new MultiTenantPlatformDbContext())
            {
                db.Add(application);
            }
        }
    }
}
