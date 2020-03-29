using SevenTiny.Bantina;
using SevenTiny.Cloud.Account.Core.Entity;
using System.Collections.Generic;

namespace SevenTiny.Cloud.Account.Core.Repository
{
    public interface ICommonInfoRepository<TEntity> : IRepository<TEntity> where TEntity : CommonInfo
    {
        Result<TEntity> Delete(int id);
        Result<TEntity> LogicDelete(int id);
        Result<TEntity> Recover(int id);
        TEntity GetById(int id);
        List<TEntity> GetEntitiesDeleted();
        List<TEntity> GetEntitiesUnDeleted();
    }
}
