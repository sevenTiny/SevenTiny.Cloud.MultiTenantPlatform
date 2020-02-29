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
            _dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext _dbContext;

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

        public List<TEntity> GetEntitiesDeleted()
            => _dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.Deleted).ToList();

        public List<TEntity> GetEntitiesUnDeleted()
            => _dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.UnDeleted).ToList();

        /// <summary>
        /// 通过名称或编码查询数据是否存在，通常用在新增操作，校验名称或编码是否已经被使用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public TEntity GetByNameOrCode(string name, string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Code.Equals(code) || t.Name.Equals(name)).FirstOrDefault();

        /// <summary>
        /// 通过名称或编码查询但是id不同的数据，通常用在修改操作，校验名称或编码是否已经被其他数据使用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public TEntity GetByNameOrCodeWithNotSameId(Guid id, string name, string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Id != id && (t.Code.Equals(code) || t.Name.Equals(name))).FirstOrDefault();

        public bool CheckExistSameNameOrCodeWithNotSameId(Guid id, string name, string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Id != id && (t.Code.Equals(code) || t.Name.Equals(name))).Any();

        /// <summary>
        /// 通过名称查询但是id不同的数据，通常用在修改操作，校验名称是否已经被其他数据使用
        /// 注：用于编码不可修改的场景
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public TEntity GetByNameWithNotSameId(Guid id, string name)
            => _dbContext.Queryable<TEntity>().Where(t => t.Id != id && t.Name.Equals(name)).FirstOrDefault();

        /// <summary>
        /// 注：用于编码不可修改的场景
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckExistSameNameWithNotSameId(Guid id, string name)
            => _dbContext.Queryable<TEntity>().Where(t => t.Id != id && t.Name.Equals(name)).Any();
    }
}
