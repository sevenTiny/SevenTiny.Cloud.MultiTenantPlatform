using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Cloud.MultiTenant.Core.DataAccess;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Enum;
using SevenTiny.Cloud.MultiTenant.Core.Repository;
using SevenTiny.Cloud.MultiTenant.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Core.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenant.Core.Service
{
    public class SearchConditionNodeService : Repository<SearchConditionNode>, ISearchConditionNodeService
    {
        public SearchConditionNodeService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IMetaFieldService _metaFieldService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            metaFieldService = _metaFieldService;
        }

        readonly MultiTenantPlatformDbContext dbContext;
        readonly IMetaFieldService metaFieldService;

        public List<SearchConditionNode> GetListBySearchConditionId(int searchConditionId)
        {
            return dbContext.Queryable<SearchConditionNode>().Where(t => t.SearchConditionId == searchConditionId).ToList();
        }

        public List<SearchConditionNode> GetConditionItemsBySearchConditionId(int searchConditionId)
        {
            return dbContext.Queryable<SearchConditionNode>().Where(t => t.SearchConditionId == searchConditionId && t.FieldId != -1).ToList();
        }

        public List<SearchConditionNode> GetParametersConditionItemsBySearchConditionId(int searchConditionId)
        {
            int valueType = (int)ConditionValueType.Parameter;
            return dbContext.Queryable<SearchConditionNode>().Where(t => t.SearchConditionId == searchConditionId && t.FieldId != -1 && t.ValueType == valueType).ToList();
        }

        public Result<SearchConditionNode> Delete(int id)
        {
            dbContext.Delete<SearchConditionNode>(t => t.Id == id);
            return Result<SearchConditionNode>.Success();
        }

        //组织接口搜索条件
        public Result<SearchConditionNode> AggregateCondition(int interfaceConditionId, int brotherNodeId, int conditionJointTypeId, int fieldId, int conditionTypeId, string conditionValue, int conditionValueTypeId)
        {
            //如果不是参数传递值，则根据传入的字段校验数据
            if (conditionValueTypeId != (int)ConditionValueType.Parameter)
            {
                if (!metaFieldService.CheckAndGetFieldValueByFieldType(fieldId, conditionValue).IsSuccess)
                    return Result<SearchConditionNode>.Error("字段值和字段定义的类型不匹配");
            }

            return TransactionHelper.Transaction(() =>
            {
                int parentId = brotherNodeId;
                //如果兄弟节点!=-1，说明当前树有值。反之，则构建新树
                if (parentId != -1)
                {
                    //判断是否有树存在
                    List<SearchConditionNode> conditionListExist = GetListBySearchConditionId(interfaceConditionId);
                    //查看当前兄弟节点的父节点id
                    SearchConditionNode brotherCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherNodeId);
                    parentId = brotherCondition.ParentId;
                    //拿到父节点的信息
                    SearchConditionNode parentCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherCondition.ParentId);
                    //如果父节点的连接条件和当前新建的条件一致，则不需要新建条件节点，直接附加在已有的条件下面
                    if (parentCondition == null || parentCondition.ConditionType != conditionJointTypeId)
                    {
                        //先添加一个父节点，然后把兄弟节点的父节点指向新建的父节点
                        string tempKey = DateTime.Now.ToString("yyyyMMddhhmmss");
                        SearchConditionNode newParentCondition = new SearchConditionNode
                        {
                            SearchConditionId = interfaceConditionId,
                            ParentId = conditionListExist.Count > 0 ? parentId : -1,//如果有树，则插入节点的父节点为刚才的兄弟节点的父节点，否则，插入-1作为根节点
                            FieldId = -1,//连接节点没有field
                            FieldCode = "-1",
                            FieldName = tempKey,
                            FieldType = -1,
                            ConditionType = conditionJointTypeId,
                            Name = ConditionJointTranslator.ToLabelWithExplain(conditionJointTypeId),
                            Value = "-1",
                            ValueType = -1
                        };
                        base.Add(newParentCondition);
                        //查询刚才插入的节点
                        newParentCondition = dbContext.Queryable<SearchConditionNode>().Where(t => t.FieldName.Contains(tempKey)).ToOne();

                        //将兄弟节点的父节点指向新插入的节点
                        brotherCondition.ParentId = newParentCondition.Id;
                        base.Update(brotherCondition);

                        //重新赋值parentId
                        parentId = newParentCondition.Id;
                    }
                }
                //检验是否没有条件节点
                if (parentId == -1)
                {
                    if (dbContext.Queryable<SearchConditionNode>().Where(t => t.Id == parentId).Any())
                    {
                        return Result<SearchConditionNode>.Error("已经存在条件节点，请查证后操作！");
                    }
                }
                //新增节点
                MetaField metaField = metaFieldService.GetById(fieldId);
                SearchConditionNode newCondition = new SearchConditionNode
                {
                    SearchConditionId = interfaceConditionId,
                    ParentId = parentId,
                    FieldId = fieldId,
                    FieldName = metaField.Name,
                    FieldCode = metaField.Code,
                    FieldType = metaField.FieldType,
                    ConditionType = conditionTypeId,
                    Name = $"{metaField.Name} {ConditionTypeTranslator.ToLabel(conditionTypeId)} {conditionValue}",
                    Value = conditionValue,
                    ValueType = conditionValueTypeId,
                    Text = metaField.Name,
                    Visible = (int)TrueFalse.True
                };
                base.Add(newCondition);

                return Result<SearchConditionNode>.Success("保存成功！");
            });
        }

        //删除某个节点
        public Result<SearchConditionNode> DeleteAggregateCondition(int nodeId, int searchConditionId)
        {
            //将要删除的节点id集合
            List<int> willBeDeleteIds = new List<int>();

            List<SearchConditionNode> allConditions = GetListBySearchConditionId(searchConditionId);
            if (allConditions == null || !allConditions.Any())
            {
                return Result<SearchConditionNode>.Success("删除成功！");
            }
            SearchConditionNode conditionAggregation = allConditions.FirstOrDefault(t => t.Id == nodeId);
            if (conditionAggregation == null)
            {
                return Result<SearchConditionNode>.Success("删除成功！");
            }
            //获取父节点id
            int parentId = conditionAggregation.ParentId;
            //如果是顶级节点，直接删除
            if (parentId == -1)
            {
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
                return Result<SearchConditionNode>.Success("删除成功！");
            }
            //如果不是顶级节点，查询所有兄弟节点。
            //如果所有兄弟节点（包含自己）多余两个，则直接删除此节点;
            List<SearchConditionNode> conditionList = allConditions.Where(t => t.ParentId == parentId).ToList();
            if (conditionList.Count > 2)
            {
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
                return Result<SearchConditionNode>.Success("删除成功！");
            }
            //如果兄弟节点为两个，则将父亲节点删除，将另一个兄弟节点作为父节点。
            else if (conditionList.Count == 2)
            {
                //获取到父节点
                SearchConditionNode parentNode = allConditions.FirstOrDefault(t => t.Id == parentId);
                //找到兄弟节点（同一个父节点，id!=当前节点）
                SearchConditionNode brotherNode = allConditions.FirstOrDefault(t => t.ParentId == parentId && t.Id != nodeId);
                //将兄弟节点的父节点指向父节点的父节点
                brotherNode.ParentId = parentNode.ParentId;
                //更新兄弟节点
                base.Update(brotherNode);
                //将父节点删除
                this.Delete(parentId);
                //删除要删除的节点以及下级节点
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
            }
            //如果没有兄弟节点，则直接将节点以及父节点都删除（如果数据不出问题，默认不存在此种情况，直接返回结果）
            else
            {
                return Result<SearchConditionNode>.Success("删除成功！");
            }

            return Result<SearchConditionNode>.Success("删除成功！");

            //删除节点级所有下级节点
            void DeleteNodeAndChildrenNodes(List<SearchConditionNode> sourceConditions, int currentNodeId)
            {
                FindDeleteNodeAndChildrenNodes(sourceConditions, currentNodeId);
                Expression<Func<SearchConditionNode, bool>> func = t => t.Id == nodeId;
                foreach (var item in willBeDeleteIds)
                {
                    func = func.Or(tt => tt.Id == item);
                }
                dbContext.Delete(func);
            }

            //遍历整棵树，找到要删除的节点以及下级节点
            void FindDeleteNodeAndChildrenNodes(List<SearchConditionNode> sourceConditions, int currentNodeId)
            {
                var children = sourceConditions.Where(t => t.ParentId == currentNodeId).ToList();
                if (children != null && children.Any())
                {
                    foreach (var item in children)
                    {
                        willBeDeleteIds.Add(item.Id);
                        FindDeleteNodeAndChildrenNodes(children, item.Id);
                    }
                }
            }
        }

        /// <summary>
        /// 将条件配置解析成mongodb可以执行的条件
        /// </summary>
        /// <param name="metaObjectId">条件id</param>
        /// <param name="searchConditionId">从http请求中传递过来的参数值集合</param>
        /// <param name="conditionValueDic">参数</param>
        /// <param name="isIgnoreArgumentsCheck">是否忽略参数校验,如果为true，需要的参数未传递会抛出异常；如果为false，需要的参数不存在条件返回null</param>
        /// <returns></returns>
        public FilterDefinition<BsonDocument> AnalysisConditionToFilterDefinitionByConditionId(QueryPiplineContext queryPiplineContext, bool isIgnoreArgumentsCheck = false)
        {
            List<SearchConditionNode> conditions = GetListBySearchConditionId(queryPiplineContext.SearchConditionId);
            return AnalysisConditionToFilterDefinition(queryPiplineContext, conditions, isIgnoreArgumentsCheck);
        }

        private FilterDefinition<BsonDocument> AnalysisConditionToFilterDefinition(QueryPiplineContext queryPiplineContext, List<SearchConditionNode> conditions, bool isIgnoreArgumentsCheck = false)
        {
            var bf = Builders<BsonDocument>.Filter;
            //全部字段字典缓存
            Dictionary<int, MetaField> metaFieldIdDic = queryPiplineContext.MetaFieldsUnDeletedIdDic;

            //获取全部条件表达式
            if (conditions == null || !conditions.Any())
            {
                return null;
            }
            SearchConditionNode condition = conditions.FirstOrDefault(t => t.ParentId == -1);
            if (condition == null)
            {
                return null;
            }

            //如果valueType==-1，则表明这个是连接条件
            if (condition.ValueType == -1)
            {
                //通过链接条件解析器进行解析
                return ConditionRouter(condition);
            }
            //valueType!=-1 则表明这是个语句
            else
            {
                //通过条件表达式语句解析器解析
                return ConditionValue(condition);
            }

            //连接条件解析器。如果是连接条件， 则执行下面逻辑将左...右子条件解析
            FilterDefinition<BsonDocument> ConditionRouter(SearchConditionNode routeCondition)
            {
                FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Empty;
                //将子节点全部取出
                var routeConditionChildren = conditions.Where(t => t.ParentId == routeCondition.Id).ToList();
                var first = routeConditionChildren.FirstOrDefault();
                if (first != null)
                {
                    //如果字节点是连接条件
                    if (first.ValueType == -1)
                    {
                        filterDefinition = ConditionRouter(first);
                    }
                    //如果是语句
                    else
                    {
                        filterDefinition = ConditionValue(first);
                        //根据根节点的连接条件执行不同的连接操作
                        switch (routeCondition.ConditionType)
                        {
                            case (int)ConditionJoint.And:
                                //子节点全部是与逻辑
                                foreach (var item in routeConditionChildren.Except(new[] { first }))
                                {
                                    //如果valueType==-1 则表明是连接条件
                                    if (item.ValueType == -1)
                                    {
                                        var tempCondition = ConditionRouter(item);
                                        if (tempCondition != null)
                                        {
                                            filterDefinition = bf.And(filterDefinition, tempCondition);
                                        }
                                    }
                                    //如果是表达式语句
                                    else
                                    {
                                        var tempCondition = ConditionValue(item);
                                        if (tempCondition != null)
                                        {
                                            filterDefinition = bf.And(filterDefinition, tempCondition);
                                        }
                                    }
                                }
                                break;
                            case (int)ConditionJoint.Or:
                                //子节点全部是或逻辑
                                foreach (var item in routeConditionChildren.Except(new[] { first }))
                                {
                                    //如果valueType==-1 则表明是连接条件
                                    if (item.ValueType == -1)
                                    {
                                        var tempCondition = ConditionRouter(item);
                                        if (tempCondition != null)
                                        {
                                            filterDefinition = bf.Or(filterDefinition, tempCondition);
                                        }
                                    }
                                    //如果是表达式语句
                                    else
                                    {
                                        var tempCondition = ConditionValue(item);
                                        if (tempCondition != null)
                                        {
                                            filterDefinition = bf.Or(filterDefinition, tempCondition);
                                        }
                                    }
                                }
                                break;
                            default:
                                return null;
                        }
                    }
                    return filterDefinition;
                }
                return null;
            }

            //条件值解析器
            FilterDefinition<BsonDocument> ConditionValue(SearchConditionNode routeCondition)
            {
                //如果条件值来自参数,则从参数列表里面获取
                if (routeCondition.Value.Equals("?"))
                {
                    //从参数获取到值
                    string key = routeCondition.FieldCode;
                    var keyUpper = key.ToUpperInvariant();
                    //如果没有传递参数值，则抛出异常
                    if (!queryPiplineContext.ArgumentsDic.ContainsKey(keyUpper))
                    {
                        //如果忽略参数检查，则直接返回null
                        if (isIgnoreArgumentsCheck)
                            return Builders<BsonDocument>.Filter.Empty;
                        //如果不忽略参数检查，则抛出异常
                        else
                            throw new ArgumentNullException(key, $"Conditions define field parameters [{key}] but do not provide values.");
                    }
                    object argumentValue = queryPiplineContext.ArgumentsDic.SafeGet(keyUpper);
                    //将值转化为字段同类型的类型值
                    object value = metaFieldService.CheckAndGetFieldValueByFieldType(metaFieldIdDic[routeCondition.FieldId], argumentValue).Data;
                    switch (routeCondition.ConditionType)
                    {
                        case (int)ConditionType.Equal:
                            return bf.Eq(key, value);
                        case (int)ConditionType.GreaterThan:
                            return bf.Gt(key, value);
                        case (int)ConditionType.GreaterThanEqual:
                            return bf.Gte(key, value);
                        case (int)ConditionType.LessThan:
                            return bf.Lt(key, value);
                        case (int)ConditionType.LessThanEqual:
                            return bf.Lte(key, value);
                        case (int)ConditionType.NotEqual:
                            return bf.Ne(key, value);
                        default:
                            return Builders<BsonDocument>.Filter.Empty;
                    }
                }
                //如果来自配置，则直接从配置里面获取到值
                else
                {
                    //校验字段以及转换字段值为目标类型
                    var convertResult = metaFieldService.CheckAndGetFieldValueByFieldType(metaFieldIdDic[routeCondition.FieldId], routeCondition.Value);
                    if (!convertResult.IsSuccess)
                    {
                        throw new ArgumentException("配置的字段值不符合字段的类型");
                    }

                    switch (routeCondition.ConditionType)
                    {
                        case (int)ConditionType.Equal:
                            return bf.Eq(routeCondition.FieldCode, convertResult.Data);
                        case (int)ConditionType.GreaterThan:
                            return bf.Gt(routeCondition.FieldCode, convertResult.Data);
                        case (int)ConditionType.GreaterThanEqual:
                            return bf.Gte(routeCondition.FieldCode, convertResult.Data);
                        case (int)ConditionType.LessThan:
                            return bf.Lt(routeCondition.FieldCode, convertResult.Data);
                        case (int)ConditionType.LessThanEqual:
                            return bf.Lte(routeCondition.FieldCode, convertResult.Data);
                        case (int)ConditionType.NotEqual:
                            return bf.Ne(routeCondition.FieldCode, convertResult.Data);
                        default:
                            return Builders<BsonDocument>.Filter.Empty;
                    }
                }
            }
        }

        public new Result<SearchConditionNode> Update(SearchConditionNode entity)
        {
            var myEntity = dbContext.Queryable<SearchConditionNode>().Where(t => t.Id == entity.Id).ToOne();
            if (myEntity != null)
            {
                myEntity.Text = entity.Text;
                myEntity.Visible = entity.Visible;
            }
            base.Update(myEntity);
            return Result<SearchConditionNode>.Success();
        }

        public SearchConditionNode GetById(int id)
        {
            return dbContext.Queryable<SearchConditionNode>().Where(t => t.Id == id).ToOne();
        }
    }
}
