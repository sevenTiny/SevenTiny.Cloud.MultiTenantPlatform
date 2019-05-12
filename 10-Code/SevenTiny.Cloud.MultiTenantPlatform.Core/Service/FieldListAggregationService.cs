using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SevenTiny.Bantina;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class FieldListAggregationService : Repository<FieldListAggregation>, IFieldListAggregationService
    {
        public FieldListAggregationService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IMetaFieldService _metaFieldService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            metaFieldService = _metaFieldService;
        }

        readonly MultiTenantPlatformDbContext dbContext;
        readonly IMetaFieldService metaFieldService;

        public new Result<IList<FieldListAggregation>> Add(IList<FieldListAggregation> entities)
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

        public List<FieldListAggregation> GetByFieldListId(int fieldListId)
        {
            return dbContext.Queryable<FieldListAggregation>().Where(t => t.FieldListId == fieldListId).ToList();
        }

        public void DeleteByMetaFieldId(int metaFieldId)
        {
            dbContext.Delete<FieldListAggregation>(t => t.MetaFieldId == metaFieldId);
        }

        public List<MetaField> GetMetaFieldsByFieldListId(int fieldListId)
        {
            var fieldAggregationList = GetByFieldListId(fieldListId);
            if (fieldAggregationList != null && fieldAggregationList.Any())
            {
                var fieldIds = fieldAggregationList.Select(t => t.MetaFieldId).ToArray();
                return metaFieldService.GetByIds(fieldIds);
            }
            return null;
        }

        public Dictionary<string, MetaField> GetMetaFieldsDicByFieldListId(int fieldListId)
        {
            return GetMetaFieldsByFieldListId(fieldListId)?.ToDictionary(t => t.Code, t => t);
        }

        public List<Column> GetColumnDataByFieldListId(int interfaceFieldId)
        {
            var fieldList = GetByFieldListId(interfaceFieldId);
            if (fieldList != null && fieldList.Any())
            {
                List<Column> columns = new List<Column>();
                foreach (var item in fieldList)
                {
                    columns.Add(new Column
                    {
                        CmpData = new ColumnCmpData
                        {
                            Name = item.Name,
                            Title = item.Text,
                            Type = item.FieldType.ToString(),
                            Visible = TrueFalseTranslator.ToBoolean(item.IsVisible),
                            IsLink = TrueFalseTranslator.ToBoolean(item.IsLink),
                            ShowIndex = item.SortNumber
                        }
                    });
                }
                return columns;
            }
            return null;
        }

        public FieldListAggregation GetById(int id)
        {
            return dbContext.Queryable<FieldListAggregation>().Where(t => t.Id == id).ToOne();
        }

        public new Result Update(FieldListAggregation entity)
        {
            var entityExist = GetById(entity.Id);
            if (entityExist != null)
            {
                entityExist.IsLink = entity.IsLink;
                entityExist.IsVisible = entity.IsVisible;
                entityExist.Text = entity.Text;
            }
            return base.Update(entityExist);
        }

        public async void SortFields(int interfaceFieldId, int[] currentOrderMetaFieldIds)
        {
            await Task.Run(() =>
            {
                var fieldList = GetByFieldListId(interfaceFieldId);
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
            });
        }
    }
}
