using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class SearchConditionNodeService : CommonServiceBase<SearchConditionNode>, ISearchConditionNodeService
    {
        public SearchConditionNodeService(IMetaFieldService metaFieldService, IMetaFieldRepository metaFieldRepository, ISearchConditionNodeRepository searchConditionNodeRepository) : base(searchConditionNodeRepository)
        {
            _searchConditionNodeRepository = searchConditionNodeRepository;
            _metaFieldRepository = metaFieldRepository;
            _metaFieldService = metaFieldService;
        }

        ISearchConditionNodeRepository _searchConditionNodeRepository;
        IMetaFieldRepository _metaFieldRepository;
        IMetaFieldService _metaFieldService;

        //组织接口搜索条件
        public Result AggregateCondition(Guid interfaceConditionId, Guid brotherNodeId, int conditionJointTypeId, Guid fieldId, int conditionTypeId, string conditionValue, int conditionValueTypeId)
        {
            //如果不是参数传递值，则根据传入的字段校验数据
            if (conditionValueTypeId != (int)ConditionValueType.Parameter)
            {
                if (!_metaFieldService.CheckAndGetFieldValueByFieldType(fieldId, conditionValue).IsSuccess)
                    return Result.Error("字段值和字段定义的类型不匹配");
            }

            return TransactionHelper.Transaction(() =>
            {
                Guid parentId = brotherNodeId;
                //如果兄弟节点!=-1，说明当前树有值。反之，则构建新树
                if (parentId != Guid.Empty)
                {
                    //判断是否有树存在
                    List<SearchConditionNode> conditionListExist = _searchConditionNodeRepository.GetListBySearchConditionId(interfaceConditionId);
                    //查看当前兄弟节点的父节点id
                    SearchConditionNode brotherCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherNodeId);
                    parentId = brotherCondition.ParentId;
                    //拿到父节点的信息
                    SearchConditionNode parentCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherCondition.ParentId);
                    //如果父节点的连接条件和当前新建的条件一致，则不需要新建条件节点，直接附加在已有的条件下面
                    if (parentCondition == null || parentCondition.ConditionType != conditionJointTypeId)
                    {
                        //先添加一个父节点，然后把兄弟节点的父节点指向新建的父节点
                        SearchConditionNode newParentCondition = new SearchConditionNode
                        {
                            Id = Guid.NewGuid(),
                            Code = Guid.Empty.ToString(),
                            SearchConditionId = interfaceConditionId,
                            ParentId = conditionListExist.Count > 0 ? parentId : Guid.Empty,//如果有树，则插入节点的父节点为刚才的兄弟节点的父节点，否则，插入-1作为根节点
                            MetaFieldId = Guid.Empty,//连接节点没有field
                            MetaFieldCode = "-1",
                            MetaFieldName = string.Empty,
                            MetaFieldType = -1,
                            ConditionType = conditionJointTypeId,
                            Name = ConditionJointTranslator.ToLabelWithExplain(conditionJointTypeId),
                            Value = "-1",
                            ValueType = -1
                        };
                        base.Add(newParentCondition);

                        //将兄弟节点的父节点指向新插入的节点
                        brotherCondition.ParentId = newParentCondition.Id;

                        _searchConditionNodeRepository.Update(brotherCondition);

                        //重新赋值parentId
                        parentId = newParentCondition.Id;
                    }
                }
                //检验是否没有条件节点
                if (parentId == Guid.Empty)
                {
                    if (_searchConditionNodeRepository.GetById(parentId) != null)
                    {
                        return Result.Error("已经存在条件节点，请查证后操作！");
                    }
                }
                //新增节点
                MetaField metaField = _metaFieldRepository.GetById(fieldId);
                SearchConditionNode newCondition = new SearchConditionNode
                {
                    Code = Guid.NewGuid().ToString(),
                    SearchConditionId = interfaceConditionId,
                    ParentId = parentId,
                    MetaFieldId = fieldId,
                    MetaFieldName = metaField.Name,
                    MetaFieldCode = metaField.Code,
                    MetaFieldType = metaField.FieldType,
                    ConditionType = conditionTypeId,
                    Name = $"{metaField.Name} {ConditionTypeTranslator.ToLabel(conditionTypeId)} {conditionValue}",
                    Value = conditionValue,
                    ValueType = conditionValueTypeId,
                    Text = metaField.Name,
                    Visible = (int)TrueFalse.True
                };
                _searchConditionNodeRepository.Add(newCondition);

                return Result.Success("保存成功！");
            });
        }

        //删除某个节点
        public Result DeleteAggregateCondition(Guid nodeId, Guid searchConditionId)
        {
            //将要删除的节点id集合
            var willBeDeleteIds = new List<Guid>();

            List<SearchConditionNode> allConditions = _searchConditionNodeRepository.GetListBySearchConditionId(searchConditionId);
            if (allConditions == null || !allConditions.Any())
            {
                return Result.Success("删除成功！");
            }
            SearchConditionNode conditionAggregation = allConditions.FirstOrDefault(t => t.Id == nodeId);
            if (conditionAggregation == null)
            {
                return Result.Success("删除成功！");
            }
            //获取父节点id
            Guid parentId = conditionAggregation.ParentId;
            //如果是顶级节点，直接删除
            if (parentId == Guid.Empty)
            {
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
                return Result.Success("删除成功！");
            }
            //如果不是顶级节点，查询所有兄弟节点。
            //如果所有兄弟节点（包含自己）多余两个，则直接删除此节点;
            List<SearchConditionNode> conditionList = allConditions.Where(t => t.ParentId == parentId).ToList();
            if (conditionList.Count > 2)
            {
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
                return Result.Success("删除成功！");
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
                _searchConditionNodeRepository.Update(brotherNode);
                //将父节点删除
                _searchConditionNodeRepository.Delete(parentId);
                //删除要删除的节点以及下级节点
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
            }
            //如果没有兄弟节点，则直接将节点以及父节点都删除（如果数据不出问题，默认不存在此种情况，直接返回结果）
            else
            {
                return Result.Success("删除成功！");
            }

            return Result.Success("删除成功！");

            //删除节点及所有下级节点
            void DeleteNodeAndChildrenNodes(List<SearchConditionNode> sourceConditions, Guid currentNodeId)
            {
                FindDeleteNodeAndChildrenNodes(sourceConditions, currentNodeId);
                _searchConditionNodeRepository.Delete(nodeId);
                foreach (var item in willBeDeleteIds)
                {
                    _searchConditionNodeRepository.Delete(item);
                }
            }

            //遍历整棵树，找到要删除的节点以及下级节点
            void FindDeleteNodeAndChildrenNodes(List<SearchConditionNode> sourceConditions, Guid currentNodeId)
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
            List<SearchConditionNode> conditions = _searchConditionNodeRepository.GetListBySearchConditionId(queryPiplineContext.SearchConditionId);
            return AnalysisConditionToFilterDefinition(queryPiplineContext, conditions, isIgnoreArgumentsCheck);
        }

        private FilterDefinition<BsonDocument> AnalysisConditionToFilterDefinition(QueryPiplineContext queryPiplineContext, List<SearchConditionNode> conditions, bool isIgnoreArgumentsCheck = false)
        {
            var bf = Builders<BsonDocument>.Filter;
            //全部字段字典缓存
            Dictionary<Guid, MetaField> metaFieldIdDic = queryPiplineContext.MetaFieldsUnDeletedIdDic;

            //获取全部条件表达式
            if (conditions == null || !conditions.Any())
            {
                return null;
            }
            SearchConditionNode condition = conditions.FirstOrDefault(t => t.ParentId == Guid.Empty);
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
                    string key = routeCondition.MetaFieldCode;
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
                    object value = _metaFieldService.CheckAndGetFieldValueByFieldType(metaFieldIdDic[routeCondition.MetaFieldId], argumentValue).Data;
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
                    var convertResult = _metaFieldService.CheckAndGetFieldValueByFieldType(metaFieldIdDic[routeCondition.MetaFieldId], routeCondition.Value);
                    if (!convertResult.IsSuccess)
                    {
                        throw new ArgumentException("配置的字段值不符合字段的类型");
                    }

                    switch (routeCondition.ConditionType)
                    {
                        case (int)ConditionType.Equal:
                            return bf.Eq(routeCondition.MetaFieldCode, convertResult.Data);
                        case (int)ConditionType.GreaterThan:
                            return bf.Gt(routeCondition.MetaFieldCode, convertResult.Data);
                        case (int)ConditionType.GreaterThanEqual:
                            return bf.Gte(routeCondition.MetaFieldCode, convertResult.Data);
                        case (int)ConditionType.LessThan:
                            return bf.Lt(routeCondition.MetaFieldCode, convertResult.Data);
                        case (int)ConditionType.LessThanEqual:
                            return bf.Lte(routeCondition.MetaFieldCode, convertResult.Data);
                        case (int)ConditionType.NotEqual:
                            return bf.Ne(routeCondition.MetaFieldCode, convertResult.Data);
                        default:
                            return Builders<BsonDocument>.Filter.Empty;
                    }
                }
            }
        }

        public new Result Update(SearchConditionNode entity)
        {
            return base.UpdateWithOutCode(entity, target => {
                target.Text = entity.Text;
                target.Visible = entity.Visible;
            });
        }

        public List<SearchConditionNode> GetListBySearchConditionId(Guid id)
        {
            return _searchConditionNodeRepository.GetListBySearchConditionId(id);
        }

        public List<SearchConditionNode> GetParameterTypeListBySearchConditionId(Guid id)
        {
            return _searchConditionNodeRepository.GetParameterTypeListBySearchConditionId(id);
        }
    }
}
