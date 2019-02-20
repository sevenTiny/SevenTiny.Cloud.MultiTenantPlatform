using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.UIMetaData.ListView;
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
                        MetaField = new MetaField {
                            Name = item.Name,
                            Text = item.Text,
                            FieldType = item.FieldType,
                            FieldLength = item.FieldLength,
                            IsVisible = TrueFalseTranslator.ToBoolean(item.IsVisible),
                            IsUrl = TrueFalseTranslator.ToBoolean(item.IsUrl)
                        }
                    });
                }
                return columns;
            }
            return null;
        }
    }
}
