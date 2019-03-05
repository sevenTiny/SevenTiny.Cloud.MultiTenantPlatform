using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Repository
{
    public interface ICommonInfoRepository<TEntity> : IRepository<TEntity> where TEntity : CommonInfo
    {
        ResultModel Delete(int id);
        ResultModel LogicDelete(int id);
        ResultModel Recover(int id);
        TEntity GetById(int id);
        TEntity GetByCode(string code);
        List<TEntity> GetEntitiesDeleted();
        List<TEntity> GetEntitiesUnDeleted();
    }
}
