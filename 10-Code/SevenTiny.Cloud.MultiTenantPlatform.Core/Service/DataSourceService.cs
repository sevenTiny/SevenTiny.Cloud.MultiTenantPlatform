using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class DataSourceService : CommonInfoRepository<DataSource>, IDataSourceService
    {
        MultiTenantPlatformDbContext _dbContext;

        public DataSourceService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            _dbContext = multiTenantPlatformDbContext;
        }

        public List<DataSource> GetListByAppIdAndDataSourceType(int applicationId, DataSourceType dataSourceType)
        {
            int dataSourceTypeIntValue = (int)dataSourceType;
            return _dbContext.Queryable<DataSource>().Where(t => t.ApplicationId == applicationId && t.DataSourceType == dataSourceTypeIntValue).ToList();
        }

        public Result CheckSameCodeOrName(DataSource entity)
        {
            var obj = _dbContext.Queryable<DataSource>().Where(t => t.Id != entity.Id && (t.Code.Equals(entity.Code) || t.Name.Equals(entity.Name))).ToOne();
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                    return Result.Error($"编码[{obj.Code}]已存在");
                else if (obj.Name.Equals(entity.Name))
                    return Result.Error($"名称[{obj.Name}]已存在");
            }
            return Result.Success();
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        public new Result<DataSource> Update(DataSource entity)
        {
            DataSource myfield = GetById(entity.Id);
            if (myfield != null)
            {
                //编码不允许修改
                myfield.Language = entity.Language;
                myfield.Script = entity.Script;

                myfield.Name = entity.Name;
                myfield.Group = entity.Group;
                myfield.SortNumber = entity.SortNumber;
                myfield.Description = entity.Description;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            return base.Update(myfield);
        }
    }
}
