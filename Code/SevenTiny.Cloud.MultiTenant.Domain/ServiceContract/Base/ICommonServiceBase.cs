using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface ICommonServiceBase<TEntity> where TEntity : CommonBase
    {
        Result Add(TEntity entity);
        Result<IList<TEntity>> BatchAdd(IList<TEntity> entities);
        Result Update(TEntity entity);
        Result Delete(TEntity entity);

        void TransactionBegin();
        void TransactionCommit();
        void TransactionRollback();

        Result CheckCodeExist(string code);
        Result CheckCodeExistWithoutSameId(Guid id, string code);
        Result UpdateWithOutCode(TEntity source, Action<TEntity> updateFieldAction = null);

        /// <summary>
        /// 只有工具才可以调用真实删除接口，业务通常调用LogicDelete接口进行逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result Delete(Guid id);
        Result LogicDelete(Guid id);
        Result Recover(Guid id);
        TEntity GetById(Guid id);
        TEntity GetByCode(string code);
        List<TEntity> GetListAll();
        List<TEntity> GetListDeleted();
        List<TEntity> GetListUnDeleted();

        TEntity GetByCodeWithoutSameId(Guid id, string code);
    }
}
