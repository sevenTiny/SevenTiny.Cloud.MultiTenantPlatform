using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Repository
{
    public interface ICommonInfoRepository<TEntity> : IRepository<TEntity> where TEntity : CommonInfo
    {
        Result<TEntity> Delete(int id);
        Result<TEntity> LogicDelete(int id);
        Result<TEntity> Recover(int id);
        TEntity GetById(int id);
        TEntity GetByCode(string code);
        List<TEntity> GetEntitiesDeleted();
        List<TEntity> GetEntitiesUnDeleted();
    }
}
