using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;
using System.Collections.Generic;

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

        public void LogicDeleteByMetaObjectId(Guid metaObjectId)
        {
            _metaObjectCommonRepositoryBase.LogicDeleteByMetaObjectId(metaObjectId);
        }

        public List<TEntity> GetListByMetaObjectId(Guid metaObjectId)
        {
            return _metaObjectCommonRepositoryBase.GetListByMetaObjectId(metaObjectId);
        }

        public List<TEntity> GetListDeletedByMetaObjectId(Guid metaObjectId)
        {
            return _metaObjectCommonRepositoryBase.GetListDeletedByMetaObjectId(metaObjectId);
        }

        public List<TEntity> GetListUnDeletedByMetaObjectId(Guid metaObjectId)
        {
            return _metaObjectCommonRepositoryBase.GetListUnDeletedByMetaObjectId(metaObjectId);
        }

        public TEntity GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(Guid metaObjectId, Guid id, string code, string name)
        {
            return _metaObjectCommonRepositoryBase.GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(metaObjectId, id, code, name);
        }
    }
}
