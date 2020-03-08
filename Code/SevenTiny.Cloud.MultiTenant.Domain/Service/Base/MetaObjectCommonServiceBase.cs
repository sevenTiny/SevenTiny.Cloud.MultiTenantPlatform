using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal abstract class MetaObjectCommonServiceBase<TEntity> : CommonServiceBase<TEntity>, IMetaObjectCommonServiceBase<TEntity> where TEntity : MetaObjectCommonBase
    {
        public MetaObjectCommonServiceBase(IMetaObjectCommonRepositoryBase<TEntity> metaObjectCommonRepositoryBase) : base(metaObjectCommonRepositoryBase)
        {
            _metaObjectCommonRepositoryBase = metaObjectCommonRepositoryBase;
        }

        protected IMetaObjectCommonRepositoryBase<TEntity> _metaObjectCommonRepositoryBase;

        /// <summary>
        /// 检查是否有相同名称的编码或名称
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Result CheckHasSameCodeOrNameWithSameMetaObjectId(Guid metaObjectId, TEntity entity)
        {
            var obj = _metaObjectCommonRepositoryBase.GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(metaObjectId, entity.Id, entity.Code, entity.Name);
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                    return Result.Error($"编码[{obj.Code}]已存在");
                else if (obj.Name.Equals(entity.Name))
                    return Result.Error($"名称[{obj.Name}]已存在");
            }
            return Result.Success();
        }
    }
}
