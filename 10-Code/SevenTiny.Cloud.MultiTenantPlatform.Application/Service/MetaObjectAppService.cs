using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Application.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application.Service
{
    public class MetaObjectAppService : IMetaObjectAppService
    {
        IMetaObjectService metaObjectService;
        IMetaFieldService metaFieldService;

        public MetaObjectAppService(IMetaObjectService _metaObjectService, IMetaFieldService _metaFieldService)
        {
            metaObjectService = _metaObjectService;
            metaFieldService = _metaFieldService;
        }

        public ResultModel AddMetaObject(int applicationId, string applicationCode, MetaObject metaObject)
        {
            //check metaobject of name or code exist?
            MetaObject obj = metaObjectService.GetMetaObjectByCodeOrNameWithApplicationId(applicationId, metaObject.Code, metaObject.Name);
            if (obj != null)
            {
                if (obj.Code.Equals(metaObject.Code))
                {
                    return ResultModel.Error("MetaObject Code Has Been Exist！", metaObject);
                }
                if (obj.Name.Equals(metaObject.Name))
                {
                    return ResultModel.Error("MetaObject Name Has Been Exist！", metaObject);
                }
            }
            if (applicationId == default(int))
            {
                return ResultModel.Error("Application Id Can Not Be Null！", metaObject);
            }

            metaObject.ApplicationId = applicationId;
            metaObject.Code = $"{applicationCode}.{metaObject.Code}";

            return TransactionHelper.Transaction(() =>
            {
                metaObjectService.Add(metaObject);

                obj = metaObjectService.GetMetaObjectByCodeAndApplicationId(metaObject.ApplicationId, metaObject.Code);

                if (obj == null)
                {
                    return ResultModel.Error("add metaobject then query faild！", metaObject);
                }

                //预置字段数据
                metaObjectService.PresetFields(obj.Id);

                return ResultModel.Success();
            });
        }

        public void Delete(int metaObjectId)
        {
            var metaObject = metaObjectService.GetById(metaObjectId);
            TransactionHelper.Transaction(() =>
            {
                metaObjectService.Delete(metaObjectId);
                //把相关字段一并删除
                metaFieldService.DeleteByMetaObjectId(metaObjectId);
            });
        }
    }
}
