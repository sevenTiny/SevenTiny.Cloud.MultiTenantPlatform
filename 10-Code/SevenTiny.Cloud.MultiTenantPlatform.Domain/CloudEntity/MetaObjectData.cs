//using SevenTiny.Cloud.MultiTenantPlatform.CloudDomain.Entity;
//using System.Collections.Generic;

//namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity
//{
//    public class MetaObjectData : CommonInfo
//    {
//        public MetaObjectData(string metaObjectCode)
//        {
//            //tode:there should be cache!!!
//            using (var db = new MultiTenantPlatformDbContext())
//            {
//                var metaObject = db.QueryOne<MetaObjectData>(t => t.Code.Equals(metaObjectCode));
//                var metaFields = db.QueryList<MetaField>(t => t.MetaObjectId == metaObject.Id);
//            }
//        }
//        public List<MetaField> MetaFields { get; set; }
//    }
//}
