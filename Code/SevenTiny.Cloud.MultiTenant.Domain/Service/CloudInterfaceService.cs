using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class CloudInterfaceService : MetaObjectCommonServiceBase<CloudInterface>, ICloudInterfaceService
    {
        public CloudInterfaceService(ICloudInterfaceRepository cloudInterfaceRepository) : base(cloudInterfaceRepository)
        {

        }

        public bool CheckFormIdExist(Guid formId)
        {
            throw new NotImplementedException();
        }

        public CloudInterface GetByInterfaceAggregationCode(string interfaceAggregationCode)
        {
            throw new NotImplementedException();
        }

        //        private void SetInterfacePropertyNameByPropertyId(ref CloudInterface entity)
        //        {
        //            switch ((InterfaceType)entity.InterfaceType)
        //            {
        //                case InterfaceType.Add:
        //                case InterfaceType.BatchAdd:
        //                    entity.FormName = _formService.GetById(entity.FormViewId)?.Name;
        //                    break;
        //                case InterfaceType.Update:
        //                    entity.FormName = _formService.GetById(entity.FormViewId)?.Name;
        //                    entity.SearchConditionName = searchConditionService.GetById(entity.SearchConditionId)?.Name;
        //                    break;
        //                case InterfaceType.Delete:
        //                    entity.SearchConditionName = searchConditionService.GetById(entity.SearchConditionId)?.Name;
        //                    break;
        //                case InterfaceType.SingleObject:
        //                case InterfaceType.TableList:
        //                    entity.FieldListName = _fieldListService.GetById(entity.ListViewId)?.Name;
        //                    entity.SearchConditionName = searchConditionService.GetById(entity.SearchConditionId)?.Name;
        //                    break;
        //                case InterfaceType.Count:
        //                    entity.SearchConditionName = searchConditionService.GetById(entity.SearchConditionId)?.Name;
        //                    break;
        //                case InterfaceType.JsonDataSource:
        //                case InterfaceType.ExecutableScriptDataSource:
        //                    entity.DataSourceName = _dataSourceService.GetById(entity.DataSourceId)?.Name;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }

        //        //新增组织接口
        //        public new Result<CloudInterface> Add(CloudInterface entity)
        //        {
        //            SetInterfacePropertyNameByPropertyId(ref entity);
        //            base.Add(entity);
        //            return Result<CloudInterface>.Success();
        //        }

        //        /// <summary>
        //        /// 更新对象
        //        /// </summary>
        //        /// <param name="interfaceAggregation"></param>
        //        public new Result<CloudInterface> Update(CloudInterface interfaceAggregation)
        //        {
        //            CloudInterface myEntity = GetById(interfaceAggregation.Id);
        //            if (myEntity != null)
        //            {
        //                myEntity.InterfaceType = interfaceAggregation.InterfaceType;
        //                myEntity.FormViewId = interfaceAggregation.FormViewId;
        //                myEntity.SearchConditionId = interfaceAggregation.SearchConditionId;
        //                myEntity.ListViewId = interfaceAggregation.ListViewId;
        //                myEntity.DataSourceId = interfaceAggregation.DataSourceId;
        //                SetInterfacePropertyNameByPropertyId(ref myEntity);

        //                //编码不允许修改
        //                myEntity.Name = interfaceAggregation.Name;
        //                myEntity.Group = interfaceAggregation.Group;
        //                myEntity.SortNumber = interfaceAggregation.SortNumber;
        //                myEntity.Description = interfaceAggregation.Description;
        //                myEntity.ModifyBy = -1;
        //                myEntity.ModifyTime = DateTime.Now;
        //            }
        //            base.Update(myEntity);
        //            return Result<CloudInterface>.Success();
        //        }

        //        public CloudInterface GetByInterfaceAggregationCode(string interfaceAggregationCode)
        //        {
        //            var interfaceAggregation = dbContext.Queryable<CloudInterface>().Where(t => t.Code.Equals(interfaceAggregationCode)).FirstOrDefault();
        //            return interfaceAggregation;
        //        }
    }
}
