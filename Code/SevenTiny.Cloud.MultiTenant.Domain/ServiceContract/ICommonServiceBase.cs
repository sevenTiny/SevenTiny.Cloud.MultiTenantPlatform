using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface ICommonServiceBase<TEntity> where TEntity : CommonBase
    {
        Result CheckCodeExist(string code);
        Result CheckCodeExistWithoutSameId(Guid id, string code);
        Result<TEntity> Add(TEntity entity);
        Result<TEntity> UpdateWithOutCode(TEntity source, Action<TEntity> updateFieldAction = null);
    }
}
