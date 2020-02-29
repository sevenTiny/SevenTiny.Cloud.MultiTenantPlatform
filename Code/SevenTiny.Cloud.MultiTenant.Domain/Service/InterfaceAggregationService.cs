using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DataAccess;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    public class InterfaceAggregationService : MetaObjectManageRepository<InterfaceAggregation>, IInterfaceAggregationService
    {
        readonly MultiTenantPlatformDbContext dbContext;
        readonly IFieldListService _fieldListService;
        readonly ISearchConditionService searchConditionService;
        readonly IFormService _formService;
        readonly IDataSourceService _dataSourceService;

        public InterfaceAggregationService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IFieldListService fieldListService,
            ISearchConditionService _searchConditionService,
            IFormService formService,
            IDataSourceService dataSourceService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            this._fieldListService = fieldListService;
            this.searchConditionService = _searchConditionService;
            _formService = formService;
            _dataSourceService = dataSourceService;
        }

        private void SetInterfacePropertyNameByPropertyId(ref InterfaceAggregation entity)
        {
            switch ((InterfaceType)entity.InterfaceType)
            {
                case InterfaceType.Add:
                case InterfaceType.BatchAdd:
                    entity.FormName = _formService.GetById(entity.FormId)?.Name;
                    break;
                case InterfaceType.Update:
                    entity.FormName = _formService.GetById(entity.FormId)?.Name;
                    entity.SearchConditionName = searchConditionService.GetById(entity.SearchConditionId)?.Name;
                    break;
                case InterfaceType.Delete:
                    entity.SearchConditionName = searchConditionService.GetById(entity.SearchConditionId)?.Name;
                    break;
                case InterfaceType.SingleObject:
                case InterfaceType.TableList:
                    entity.FieldListName = _fieldListService.GetById(entity.FieldListId)?.Name;
                    entity.SearchConditionName = searchConditionService.GetById(entity.SearchConditionId)?.Name;
                    break;
                case InterfaceType.Count:
                    entity.SearchConditionName = searchConditionService.GetById(entity.SearchConditionId)?.Name;
                    break;
                case InterfaceType.JsonDataSource:
                case InterfaceType.ExecutableScriptDataSource:
                    entity.DataSourceName = _dataSourceService.GetById(entity.DataSourceId)?.Name;
                    break;
                default:
                    break;
            }
        }

        //新增组织接口
        public new Result<InterfaceAggregation> Add(InterfaceAggregation entity)
        {
            SetInterfacePropertyNameByPropertyId(ref entity);
            base.Add(entity);
            return Result<InterfaceAggregation>.Success();
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="interfaceAggregation"></param>
        public new Result<InterfaceAggregation> Update(InterfaceAggregation interfaceAggregation)
        {
            InterfaceAggregation myEntity = GetById(interfaceAggregation.Id);
            if (myEntity != null)
            {
                myEntity.InterfaceType = interfaceAggregation.InterfaceType;
                myEntity.FormId = interfaceAggregation.FormId;
                myEntity.SearchConditionId = interfaceAggregation.SearchConditionId;
                myEntity.FieldListId = interfaceAggregation.FieldListId;
                myEntity.DataSourceId = interfaceAggregation.DataSourceId;
                SetInterfacePropertyNameByPropertyId(ref myEntity);

                //编码不允许修改
                myEntity.Name = interfaceAggregation.Name;
                myEntity.Group = interfaceAggregation.Group;
                myEntity.SortNumber = interfaceAggregation.SortNumber;
                myEntity.Description = interfaceAggregation.Description;
                myEntity.ModifyBy = -1;
                myEntity.ModifyTime = DateTime.Now;
            }
            base.Update(myEntity);
            return Result<InterfaceAggregation>.Success();
        }

        public InterfaceAggregation GetByInterfaceAggregationCode(string interfaceAggregationCode)
        {
            var interfaceAggregation = dbContext.Queryable<InterfaceAggregation>().Where(t => t.Code.Equals(interfaceAggregationCode)).ToOne();
            return interfaceAggregation;
        }
    }
}
