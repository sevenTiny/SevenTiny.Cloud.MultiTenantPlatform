using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Application.Service
{
    internal class MetaObjectAppService : IMetaObjectAppService
    {
        public MetaObjectAppService(
            IMetaObjectService metaObjectService,

            IMetaObjectRepository metaObjectRepository,
            IMetaFieldRepository metaFieldRepository,
            IListViewRepository listViewRepository,
            ICloudInterfaceRepository cloudInterfaceRepository,
            ISearchConditionRepository searchConditionRepository,
            ITriggerScriptRepository triggerScriptRepository,
            IFormViewRepository formRepository
            )
        {
            _metaObjectService = metaObjectService;

            _metaObjectRepository = metaObjectRepository;
            _metaFieldRepository = metaFieldRepository;
            _listViewRepository = listViewRepository;
            _cloudInterfaceRepository = cloudInterfaceRepository;
            _searchConditionRepository = searchConditionRepository;
            _triggerScriptRepository = triggerScriptRepository;
            _formRepository = formRepository;
        }

        IMetaObjectService _metaObjectService;

        IMetaObjectRepository _metaObjectRepository;
        IMetaFieldRepository _metaFieldRepository;
        IListViewRepository _listViewRepository;
        ICloudInterfaceRepository _cloudInterfaceRepository;
        ISearchConditionRepository _searchConditionRepository;
        ITriggerScriptRepository _triggerScriptRepository;
        IFormViewRepository _formRepository;

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [Obsolete("删除的能力放在工具，业务删除，请直接调用逻辑删除")]
        public Result<MetaObject> Delete(Guid id)
        {
            var metaObject = _metaObjectRepository.GetById(id);

            return Result<MetaObject>.Success()
               .ContinueWithTryCatch(_ =>
               {
                   _metaObjectRepository.TransactionBegin();

                   //删除字段
                   _metaFieldRepository.LogicDeleteByMetaObjectId(id);
                   //删除列表
                   _listViewRepository.LogicDeleteByMetaObjectId(id);//删除相关子对象
                                                                 //删除接口
                   _cloudInterfaceRepository.LogicDeleteByMetaObjectId(id);
                   //删除搜索条件
                   _searchConditionRepository.LogicDeleteByMetaObjectId(id);//删除相关子对象
                                                                       //删除触发器
                   _triggerScriptRepository.LogicDeleteByMetaObjectId(id);
                   //删除表单
                   _formRepository.LogicDeleteByMetaObjectId(id);
                   //这里要补充待删除的子对象
                   //...

                   //最后删除对象
                   _metaObjectRepository.Delete(id);


                   _metaObjectRepository.TransactionCommit();

                   return _;
               }, _ =>
               {
                   _metaObjectRepository.TransactionRollback();
               });
        }
    }
}