using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DataAccess;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    public class CommonRepositoryBase<TEntity> : RepositoryBase<TEntity>, ICommonRepositoryBase<TEntity> where TEntity : CommonBase
    {
        public CommonRepositoryBase(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
        }

        public new Result<TEntity> Update(TEntity entity)
        {
            entity.ModifyTime = DateTime.Now;

            base.Update(entity);
            return Result<TEntity>.Success();
        }

        public Result<TEntity> Delete(Guid id)
        {
            _dbContext.Delete<TEntity>(t => t.Id.Equals(id));
            return Result<TEntity>.Success();
        }

        public Result<TEntity> LogicDelete(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.Deleted;
                _dbContext.Update(entity);
            }
            return Result<TEntity>.Success();
        }

        public Result<TEntity> Recover(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.UnDeleted;
                _dbContext.Update(entity);
            }
            return Result<TEntity>.Success();
        }

        public TEntity GetById(Guid id)
            => _dbContext.Queryable<TEntity>().Where(t => t.Id.Equals(id)).FirstOrDefault();

        public TEntity GetByCode(string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Code.Equals(code)).FirstOrDefault();

        public List<TEntity> GetListDeleted()
            => _dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.Deleted).ToList();

        public List<TEntity> GetListUnDeleted()
            => _dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.UnDeleted).ToList();

        /// <summary>
        /// 通过编码查询但是id不同的数据，通常用在修改编码操作，校验编码是否已经被其他数据使用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public TEntity GetByCodeWithoutSameId(Guid id, string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Id != id && t.Code.Equals(code)).FirstOrDefault();

        public bool CheckCodeExistWithoutSameId(Guid id, string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Id != id && t.Code.Equals(code)).Any();

        public bool CheckCodeExist(string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Code.Equals(code)).Any();
    }
}
