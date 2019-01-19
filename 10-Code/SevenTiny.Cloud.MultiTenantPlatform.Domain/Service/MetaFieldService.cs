using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class MetaFieldService : CommonInfoRepository<MetaField>, IMetaFieldService
    {
        public MetaFieldService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public List<MetaField> GetMetaFeildsUnDeletedByMetaObjectId(int metaObjectId)
            => dbContext.QueryList<MetaField>(t => t.MetaObjectId == metaObjectId && t.IsDeleted == (int)IsDeleted.UnDeleted);

        public List<MetaField> GetMetaFeildsDeletedByMetaObjectId(int metaObjectId)
        => dbContext.QueryList<MetaField>(t => t.MetaObjectId == metaObjectId && t.IsDeleted == (int)IsDeleted.Deleted);

        /// <summary>
        /// 清空对象下的全部字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        public void DeleteByMetaObjectId(int metaObjectId)
        {
            dbContext.Delete<MetaField>(t => t.MetaObjectId == metaObjectId);
        }

        /// <summary>
        /// 检查是否有相同名称的编码或名称
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel CheckSameCodeOrName(int metaObjectId, MetaField metaField)
        {
            var obj = dbContext.QueryOne<MetaField>(t => t.MetaObjectId == metaObjectId && (t.Code.Equals(metaField.Code) || t.Name.Equals(metaField.Name)));
            if (obj != null)
            {
                if (obj.Code.Equals(metaField.Code))
                    return ResultModel.Error($"编码[{obj.Code}]已存在", metaField);
                else if (obj.Name.Equals(metaField.Name))
                    return ResultModel.Error($"名称[{obj.Name}]已存", metaField);
            }
            return ResultModel.Success();
        }
    }
}
