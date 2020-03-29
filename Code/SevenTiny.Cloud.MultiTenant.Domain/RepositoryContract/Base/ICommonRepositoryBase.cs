using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    internal interface ICommonRepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : CommonBase
    {
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
        bool CheckCodeExist(string code);
        bool CheckCodeExistWithoutSameId(Guid id, string code);
    }
}
