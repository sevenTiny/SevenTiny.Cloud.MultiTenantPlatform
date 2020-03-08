using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal abstract class CommonServiceBase<TEntity> : ICommonServiceBase<TEntity> where TEntity : CommonBase
    {
        public CommonServiceBase(ICommonRepositoryBase<TEntity> commonRepositoryBase)
        {
            _commonRepositoryBase = commonRepositoryBase;
        }

        protected ICommonRepositoryBase<TEntity> _commonRepositoryBase;

        /// <summary>
        /// 检查是否编码已经存在
        /// 常用于新增操作
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Result CheckCodeExist(string code)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(code, nameof(code))
                .ContinueAssert(_ => !_commonRepositoryBase.CheckCodeExist(code), $"编码[{code}]已存在");
        }

        /// <summary>
        /// 检查是否存在其他相同编码的记录
        /// 常用于编辑操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Result CheckCodeExistWithoutSameId(Guid id, string code)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(code, nameof(code))
                .ContinueAssert(_ => !_commonRepositoryBase.CheckCodeExistWithoutSameId(id, code), $"编码[{code}]已存在");
        }

        public Result Add(TEntity entity)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                //校验编码是否已经存在
                .Continue(_ => CheckCodeExist(entity.Code))
                //校验Id并赋值
                .Continue(_ =>
                {
                    if (entity.Id == Guid.Empty)
                        entity.Id = Guid.NewGuid();
                    return _;
                })
                .Continue(_ => _commonRepositoryBase.Add(entity));
        }

        /// <summary>
        /// 通用编辑
        /// 注：编码不能修改，其他通用字段内部修改，个性字段赋值通过action手动修改
        /// </summary>
        /// <param name="source"></param>
        /// <param name="updateFieldAction"></param>
        /// <returns></returns>
        public Result UpdateWithOutCode(TEntity source, Action<TEntity> updateFieldAction = null)
        {
            TEntity target = _commonRepositoryBase.GetById(source.Id);

            if (target == null)
                return Result.Error($"没有查到Id[{source.Id}]对应的数据.");

            //个性化字段赋值
            updateFieldAction?.Invoke(target);

            //编码不允许修改!
            target.Name = source.Name;
            target.Group = source.Group;
            target.SortNumber = source.SortNumber;
            target.Description = source.Description;
            target.ModifyBy = source.ModifyBy;
            target.ModifyTime = DateTime.Now;

            return _commonRepositoryBase.Update(target);
        }
    }
}
