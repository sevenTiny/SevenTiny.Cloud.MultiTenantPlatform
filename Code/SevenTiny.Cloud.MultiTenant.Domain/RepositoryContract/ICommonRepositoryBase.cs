using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    public interface ICommonRepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : CommonBase
    {
        Result<TEntity> Delete(Guid id);
        Result<TEntity> LogicDelete(Guid id);
        Result<TEntity> Recover(Guid id);
        TEntity GetById(Guid id);
        TEntity GetByCode(string code);
        List<TEntity> GetListDeleted();
        List<TEntity> GetListUnDeleted();

        TEntity GetByCodeWithoutSameId(Guid id, string code);
        bool CheckCodeExist(string code);
        bool CheckCodeExistWithoutSameId(Guid id, string code);
    }
}
