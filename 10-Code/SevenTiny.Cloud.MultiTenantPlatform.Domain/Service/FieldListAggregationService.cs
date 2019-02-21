using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.UIMetaData.ListView;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
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

        public new ResultModel Add(IList<FieldListAggregation> entities)
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
            return dbContext.QueryList<FieldListAggregation>(t => t.FieldListId == fieldListId);
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
                            IsLink = TrueFalseTranslator.ToBoolean(item.IsLink)
                        }
                    });
                }
                return columns;
            }
            return null;
        }

        public FieldListAggregation GetById(int id)
        {
            return dbContext.QueryOne<FieldListAggregation>(t => t.Id == id);
        }

        public new ResultModel Update(FieldListAggregation entity)
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
    }
}
