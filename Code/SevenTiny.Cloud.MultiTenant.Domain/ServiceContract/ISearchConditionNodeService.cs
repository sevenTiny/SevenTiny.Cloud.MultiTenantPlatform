using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface ISearchConditionNodeService : ICommonServiceBase<SearchConditionNode>
    {
        Result AggregateCondition(Guid interfaceConditionId, Guid brotherNodeId, int conditionJointTypeId, Guid fieldId, int conditionTypeId, string conditionValue, int conditionValueTypeId);

        Result DeleteAggregateCondition(Guid nodeId, Guid searchConditionId);

        /// <summary>
        /// 将条件配置解析成mongodb可以执行的条件
        /// </summary>
        /// <param name="metaObjectId">条件id</param>
        /// <param name="searchConditionId">从http请求中传递过来的参数值集合</param>
        /// <param name="conditionValueDic">参数</param>
        /// <param name="isIgnoreArgumentsCheck">是否忽略参数校验,如果为true，需要的参数未传递会抛出异常；如果为false，需要的参数不存在条件返回null</param>
        /// <returns></returns>
        FilterDefinition<BsonDocument> AnalysisConditionToFilterDefinitionByConditionId(QueryPiplineContext queryPiplineContext, bool isIgnoreArgumentsCheck = false);
        List<SearchConditionNode> GetListBySearchConditionId(Guid id);
        List<SearchConditionNode> GetParameterTypeListBySearchConditionId(Guid id);
    }
}
