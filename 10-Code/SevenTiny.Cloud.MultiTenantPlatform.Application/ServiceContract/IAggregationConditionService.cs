using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract
{
    public interface IAggregationConditionService
    {
        /// <summary>
        /// 将条件转换成Lambda表达式
        /// </summary>
        /// <param name="interfaceSearchConditionId"></param>
        /// <returns></returns>
        Expression<Func<ConditionAggregation, bool>> AnalysisConditionToLambda(int interfaceSearchConditionId, Dictionary<string, object> conditionValueDic);
    }
}
