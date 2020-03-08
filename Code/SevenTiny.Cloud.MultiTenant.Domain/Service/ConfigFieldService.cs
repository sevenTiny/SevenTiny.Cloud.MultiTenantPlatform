using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class ConfigFieldService : MetaObjectCommonServiceBase<ConfigField>, IConfigFieldService
    {
        public ConfigFieldService(IConfigFieldRepository configFieldRepository) : base(configFieldRepository)
        {
        }

        //public Result<IList<ListViewField>> Add(int metaObjectId, IList<ListViewField> entities)
        //{
        //    var metaFieldIds = entities.Select(t => t.MetaFieldId).ToArray();
        //    var metaFields = metaFieldService.GetByIds(metaObjectId, metaFieldIds);
        //    foreach (var item in entities)
        //    {
        //        var meta = metaFields.FirstOrDefault(t => t.Id == item.MetaFieldId);
        //        if (meta != null)
        //        {
        //            item.Name = meta.Code;
        //            item.Text = meta.Name;
        //            item.FieldType = meta.FieldType;
        //        }
        //    }
        //    return base.BatchAdd(entities);
        //}

        //public List<ListViewField> GetByFieldListId(int fieldListId)
        //{
        //    return dbContext.Queryable<ListViewField>().Where(t => t.ListViewId == fieldListId).ToList();
        //}

        //public void DeleteByMetaFieldId(int metaFieldId)
        //{
        //    dbContext.Delete<ListViewField>(t => t.MetaFieldId == metaFieldId);
        //}

        //public List<Column> GetColumnDataByFieldListId(QueryPiplineContext queryPiplineContext)
        //{
        //    var fieldListMetaFields = queryPiplineContext.FieldListMetaFieldsOfFieldListId;
        //    if (fieldListMetaFields != null && fieldListMetaFields.Any())
        //    {
        //        List<Column> columns = new List<Column>();
        //        foreach (var item in fieldListMetaFields)
        //        {
        //            columns.Add(new Column
        //            {
        //                CmpData = new ColumnCmpData
        //                {
        //                    Name = item.Name,
        //                    Title = item.Text,
        //                    Type = item.FieldType.ToString(),
        //                    Visible = TrueFalseTranslator.ToBoolean(item.IsVisible),
        //                    IsLink = TrueFalseTranslator.ToBoolean(item.IsLink),
        //                    ShowIndex = item.SortNumber
        //                }
        //            });
        //        }
        //        return columns;
        //    }
        //    return null;
        //}

        //public ListViewField GetById(int id)
        //{
        //    return dbContext.Queryable<ListViewField>().Where(t => t.Id == id).FirstOrDefault();
        //}

        //public new Result<ListViewField> Update(ListViewField entity)
        //{
        //    var entityExist = GetById(entity.Id);
        //    if (entityExist != null)
        //    {
        //        entityExist.IsLink = entity.IsLink;
        //        entityExist.IsVisible = entity.IsVisible;
        //        entityExist.Text = entity.Text;
        //    }
        //    return base.Update(entityExist);
        //}

        //public void SortFields(int interfaceFieldId, int[] currentOrderMetaFieldIds)
        //{
        //    //异步方法mysql超时!!!
        //    //await Task.Run(() =>
        //    //{
        //    var fieldList = GetByFieldListId(interfaceFieldId);
        //    if (fieldList != null && fieldList.Any())
        //    {
        //        //i为当前应该保持的顺序
        //        for (int i = 0; i < currentOrderMetaFieldIds.Length; i++)
        //        {
        //            //寻找第i个字段
        //            var item = fieldList.FirstOrDefault(t => t.MetaFieldId == currentOrderMetaFieldIds[i]);
        //            //如果该字段的排序值!=当前应该保持的顺序，则加到更新队列
        //            if (item != null && item.SortNumber != i)
        //            {
        //                item.SortNumber = i;
        //                base.Update(item);
        //            }
        //        }
        //    }
        //    //});
        //}
    }
}
