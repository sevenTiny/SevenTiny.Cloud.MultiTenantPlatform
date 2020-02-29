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
        List<TEntity> GetEntitiesDeleted();
        List<TEntity> GetEntitiesUnDeleted();
        TEntity GetByNameOrCode(string name, string code);
        TEntity GetByNameOrCodeWithNotSameId(Guid id, string name, string code);
        bool CheckExistSameNameOrCodeWithNotSameId(Guid id, string name, string code);
        TEntity GetByNameWithNotSameId(Guid id, string name);
        bool CheckExistSameNameWithNotSameId(Guid id, string name);
    }
}
