using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Application.Service
{
    internal class MetaObjectAppService : IMetaObjectAppService
    {
        public MetaObjectAppService(
            IMetaObjectService metaObjectService,

            IMetaFieldService metaFieldService,
            IListViewService listViewService,
            ICloudInterfaceService cloudInterfaceService,
            ISearchConditionService searchConditionService,
            ITriggerScriptService triggerScriptService,
            IFormViewService formService
            )
        {
            _metaObjectService = metaObjectService;
            _metaFieldService = metaFieldService;
            _listViewService = listViewService;
            _cloudInterfaceService = cloudInterfaceService;
            _searchConditionService = searchConditionService;
            _triggerScriptService = triggerScriptService;
            _formService = formService;
        }

        IMetaObjectService _metaObjectService;

        IMetaFieldService _metaFieldService;
        IListViewService _listViewService;
        ICloudInterfaceService _cloudInterfaceService;
        ISearchConditionService _searchConditionService;
        ITriggerScriptService _triggerScriptService;
        IFormViewService _formService;

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [Obsolete("删除的能力放在工具，业务删除，请直接调用逻辑删除")]
        public Result<MetaObject> Delete(Guid id)
        {
            var metaObject = _metaObjectService.GetById(id);

            return Result<MetaObject>.Success()
               .ContinueWithTryCatch(_ =>
               {
                   _metaObjectService.TransactionBegin();

                   //删除字段
                   _metaFieldService.LogicDeleteByMetaObjectId(id);
                   //删除列表
                   _listViewService.LogicDeleteByMetaObjectId(id);//删除相关子对象
                                                                 //删除接口
                   _cloudInterfaceService.LogicDeleteByMetaObjectId(id);
                   //删除搜索条件
                   _searchConditionService.LogicDeleteByMetaObjectId(id);//删除相关子对象
                                                                       //删除触发器
                   _triggerScriptService.LogicDeleteByMetaObjectId(id);
                   //删除表单
                   _formService.LogicDeleteByMetaObjectId(id);
                   //这里要补充待删除的子对象
                   //...

                   //最后删除对象
                   _metaObjectService.Delete(id);


                   _metaObjectService.TransactionCommit();

                   return _;
               }, _ =>
               {
                   _metaObjectService.TransactionRollback();
               });
        }
    }
}