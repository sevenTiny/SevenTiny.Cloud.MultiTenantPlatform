using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository
{
    public interface ICommonInfoRepository<TEntity> : IRepository<TEntity> where TEntity : CommonInfo
    {
        void Delete(int id);
        void LogicDelete(int id);
        void Recover(int id);
        TEntity GetById(int id);
        TEntity GetByCode(string code);
        List<TEntity> GetEntitiesDeleted();
        List<TEntity> GetEntitiesUnDeleted();
    }
}
