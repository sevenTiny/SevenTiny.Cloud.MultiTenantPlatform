using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    public class CommonServiceBase<TEntity> : ICommonServiceBase<TEntity> where TEntity : CommonBase
    {
        public CommonServiceBase(ICommonRepositoryBase<TEntity> commonInfoRepositoryBase)
        {
            _commonInfoRepositoryBase = commonInfoRepositoryBase;
        }

        ICommonRepositoryBase<TEntity> _commonInfoRepositoryBase;

        /// <summary>
        /// 检查是否存在其他相同编码或者名称的记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Result CheckExistOtherSameCodeOrName(TEntity entity)
        {
            var obj = _commonInfoRepositoryBase.GetByNameOrCodeWithNotSameId(entity.Id, entity.Name, entity.Code);
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                    return Result.Error($"编码[{obj.Code}]已存在");
                else if (obj.Name.Equals(entity.Name))
                    return Result.Error($"名称[{obj.Name}]已存在");
            }
            return Result.Success();
        }

        public Result<TEntity> Add(TEntity entity)
        {
            //check name or code exist?
            var obj = _commonInfoRepositoryBase.GetByNameOrCode(entity.Name, entity.Code);
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                    return Result<TEntity>.Error("Code Has Been Exist！", entity);

                if (obj.Name.Equals(entity.Name))
                    return Result<TEntity>.Error("Name Has Been Exist！", entity);
            }

            _commonInfoRepositoryBase.Add(entity);

            return Result<TEntity>.Success();
        }
    }
}
