using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class ListViewService : MetaObjectCommonServiceBase<ListView>, IListViewService
    {
        public ListViewService(IListViewRepository listViewRepository) : base(listViewRepository)
        {
            
        }


        ///// <summary>
        ///// 更新对象
        ///// </summary>
        ///// <param name="metaField"></param>
        //public new Result<ListView> Update(ListView interfaceField)
        //{
        //    ListView myfield = GetById(interfaceField.Id);
        //    if (myfield != null)
        //    {
        //        //编码不允许修改
        //        myfield.Name = interfaceField.Name;
        //        myfield.Group = interfaceField.Group;
        //        myfield.SortNumber = interfaceField.SortNumber;
        //        myfield.Description = interfaceField.Description;
        //        myfield.ModifyBy = -1;
        //        myfield.ModifyTime = DateTime.Now;
        //    }
        //    base.Update(myfield);
        //    return Result<ListView>.Success();
        //}

        ///// <summary>
        ///// 根据id删除配置字段，校验是否被引用
        ///// </summary>
        ///// <param name="id"></param>
        //public new Result<ListView> Delete(int id)
        //{
        //    if (dbContext.Queryable<CloudInterface>().Where(t => t.ListViewId == id).Any())
        //    {
        //        //存在引用关系，先删除引用该数据的数据
        //        return Result<ListView>.Error("存在引用关系，先删除引用该数据的数据");
        //    }
        //    else
        //    {
        //        base.Delete(id);
        //        return Result<ListView>.Success();
        //    }
        //}

        //public new void DeleteByMetaObjectId(int metaObjectId)
        //{
        //    var fieldLists = base.GetEntitiesByMetaObjectId(metaObjectId);

        //    TransactionHelper.Transaction(() =>
        //    {
        //        if (fieldLists != null && fieldLists.Any())
        //        {
        //            //删除字段配置下的所有字段
        //            foreach (var item in fieldLists)
        //            {
        //                dbContext.Delete<ListViewField>(t => t.ListViewId == item.Id);
        //            }
        //        }
        //        //删除字段配置
        //        base.DeleteByMetaObjectId(metaObjectId);
        //    });
        //}
    }
}
