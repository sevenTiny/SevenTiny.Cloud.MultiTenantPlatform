using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class MetaObjectRepository: IRepository<MetaObject>
    {
        public List<MetaObject> GetList(Expression<Func<MetaObject, bool>> filter)
        {
            using (var db = new MultiTenantPlatformDbContext())
            {
                return db.QueryList<MetaObject>(filter);
            }
        }

        public MetaObject GetEntity(Expression<Func<MetaObject, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void Add(MetaObject entity)
        {
            using (var db = new MultiTenantPlatformDbContext())
            {
                db.Add(entity);
            }
        }

        public void Update(Expression<Func<MetaObject, bool>> filter, MetaObject entity)
        {
            throw new NotImplementedException();
        }

        public void LogicDelete(Expression<Func<MetaObject, bool>> filter, MetaObject entity)
        {
            throw new NotImplementedException();
        }

        public void Recover(Expression<Func<MetaObject, bool>> filter, MetaObject entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<MetaObject, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}
