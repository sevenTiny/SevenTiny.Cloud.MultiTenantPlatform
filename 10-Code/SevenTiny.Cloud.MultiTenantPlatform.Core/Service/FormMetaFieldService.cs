using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class FormMetaFieldService : Repository<FormMetaField>, IFormMetaFieldService
    {
        public FormMetaFieldService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IMetaFieldService _metaFieldService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            metaFieldService = _metaFieldService;
        }

        readonly MultiTenantPlatformDbContext dbContext;
        readonly IMetaFieldService metaFieldService;

        public new Result<IList<FormMetaField>> Add(IList<FormMetaField> entities)
        {
            var metaFieldIds = entities.Select(t => t.MetaFieldId).ToArray();
            var metaFields = metaFieldService.GetByIds(metaFieldIds);
            foreach (var item in entities)
            {
                var meta = metaFields.FirstOrDefault(t => t.Id == item.MetaFieldId);
                if (meta != null)
                {
                    item.Name = meta.Code;
                    item.Text = meta.Name;
                    item.FieldType = meta.FieldType;
                }
            }
            return base.Add(entities);
        }

        public List<FormMetaField> GetByFormId(int formId)
        {
            return dbContext.Queryable<FormMetaField>().Where(t => t.FormId == formId).ToList();
        }

        public void DeleteByMetaFieldId(int metaFieldId)
        {
            dbContext.Delete<FormMetaField>(t => t.MetaFieldId == metaFieldId);
        }

        public List<MetaField> GetMetaFieldsByFormId(int fieldListId)
        {
            var fieldAggregationList = GetByFormId(fieldListId);
            if (fieldAggregationList != null && fieldAggregationList.Any())
            {
                var fieldIds = fieldAggregationList.Select(t => t.MetaFieldId).ToArray();
                return metaFieldService.GetByIds(fieldIds);
            }
            return null;
        }

        public Dictionary<string, MetaField> GetMetaFieldsDicByFormId(int fieldListId)
        {
            return GetMetaFieldsByFormId(fieldListId)?.ToDictionary(t => t.Code, t => t);
        }

        public FormMetaField GetById(int id)
        {
            return dbContext.Queryable<FormMetaField>().Where(t => t.Id == id).ToOne();
        }

        public new Result<FormMetaField> Update(FormMetaField entity)
        {
            var entityExist = GetById(entity.Id);
            if (entityExist != null)
            {
                entityExist.Text = entity.Text;
                entityExist.IsVisible = entity.IsVisible;
                entityExist.IsMust = entity.IsMust;
                entityExist.Regular = entity.Regular;
            }
            return base.Update(entityExist);
        }

        public void SortFields(int interfaceFieldId, int[] currentOrderMetaFieldIds)
        {
            //异步方法mysql超时!!!
            //await Task.Run(() =>
            //{
            var fieldList = GetByFormId(interfaceFieldId);
            if (fieldList != null && fieldList.Any())
            {
                //i为当前应该保持的顺序
                for (int i = 0; i < currentOrderMetaFieldIds.Length; i++)
                {
                    //寻找第i个字段
                    var item = fieldList.FirstOrDefault(t => t.MetaFieldId == currentOrderMetaFieldIds[i]);
                    //如果该字段的排序值!=当前应该保持的顺序，则加到更新队列
                    if (item != null && item.SortNumber != i)
                    {
                        item.SortNumber = i;
                        base.Update(item);
                    }
                }
            }
            //});
        }
    }
}
