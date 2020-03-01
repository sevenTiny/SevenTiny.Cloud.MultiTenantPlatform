using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Application.Service
{
    public class MetaObjectServiceAppService : IMetaObjectServiceAppService
    {
        public MetaObjectServiceAppService(
            IMetaObjectService metaObjectService,
            IMetaObjectRepository metaObjectRepository,
            IMetaFieldRepository metaFieldRepository,
            IFieldListRepository fieldListRepository,
            IInterfaceAggregationRepository interfaceAggregationRepository,
            ISearchConditionRepository searchConditionRepository,
            ITriggerScriptRepository triggerScriptRepository,
            IFormRepository formRepository
            )
        {
            _metaObjectService = metaObjectService;
            _metaObjectRepository = metaObjectRepository;
            _metaFieldRepository = metaFieldRepository;
            _fieldListRepository = fieldListRepository;
            _interfaceAggregationRepository = interfaceAggregationRepository;
            _searchConditionRepository = searchConditionRepository;
            _triggerScriptRepository = triggerScriptRepository;
            _formRepository = formRepository;
        }

        IMetaObjectService _metaObjectService;
        IMetaObjectRepository _metaObjectRepository;
        IMetaFieldRepository _metaFieldRepository;
        IFieldListRepository _fieldListRepository;
        IInterfaceAggregationRepository _interfaceAggregationRepository;
        ISearchConditionRepository _searchConditionRepository;
        ITriggerScriptRepository _triggerScriptRepository;
        IFormRepository _formRepository;

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public Result<MetaObject> Delete(Guid id)
        {
            var metaObject = _metaObjectRepository.GetById(id);
            TransactionHelper.Transaction(() =>
            {
                //删除字段
                _metaFieldRepository.DeleteByMetaObjectId(id);
                //删除列表
                _fieldListRepository.DeleteByMetaObjectId(id);//删除相关子对象
                //删除接口
                _interfaceAggregationRepository.DeleteByMetaObjectId(id);
                //删除搜索条件
                _searchConditionRepository.DeleteByMetaObjectId(id);//删除相关子对象
                //删除触发器
                _triggerScriptRepository.DeleteByMetaObjectId(id);
                //删除表单
                _formRepository.DeleteByMetaObjectId(id);
                //这里要补充待删除的子对象
                _metaObjectRepository.Delete(id);
            });
            return Result<MetaObject>.Success();
        }
    }
}