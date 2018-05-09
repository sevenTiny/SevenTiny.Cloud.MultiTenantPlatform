using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class MetaObjectRepository
    {
        public static List<MetaObject> GetMetaObjects()
        {
            using (var db = new MultiTenantPlatformDbContext())
            {
                return db.QueryList<MetaObject>();
            }
        }
        public static List<MetaObject> GetMetaObjects(Expression<Func<MetaObject,bool>> filter)
        {
            using (var db = new MultiTenantPlatformDbContext())
            {
                return db.QueryList<MetaObject>(filter);
            }
        }
        public static void AddMetaObject(MetaObject application)
        {
            using (var db = new MultiTenantPlatformDbContext())
            {
                db.Add(application);
            }
        }
    }
}
