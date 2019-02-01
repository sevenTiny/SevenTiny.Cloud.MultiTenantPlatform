using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface ISearchConditionAggregationService : IRepository<SearchConditionAggregation>
    {
        ResultModel Delete(int id);
        List<SearchConditionAggregation> GetListByInterfaceConditionId(int searchConditionId);
        ResultModel AggregateCondition(int interfaceConditionId, int brotherNodeId, int conditionJointTypeId, int fieldId, int conditionTypeId, string conditionValue, int conditionValueTypeId);

        ResultModel DeleteAggregateCondition(int nodeId, int searchConditionId);
        /// <summary>
        /// 将条件配置解析成mongodb可以执行的条件
        /// </summary>
        /// <param name="searchConditionId">条件id</param>
        /// <param name="conditionValueDic">从http请求中传递过来的参数值集合</param>
        /// <returns></returns>
        FilterDefinition<BsonDocument> AnalysisConditionToFilterDefinition(int metaObjectId, int searchConditionId, Dictionary<string, object> conditionValueDic);
    }
}
