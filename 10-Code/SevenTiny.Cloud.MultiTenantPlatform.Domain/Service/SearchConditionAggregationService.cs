using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Bankinate.Helpers;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class SearchConditionAggregationService : Repository<SearchConditionAggregation>, ISearchConditionAggregationService
    {
        public SearchConditionAggregationService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IMetaFieldService _metaFieldService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            metaFieldService = _metaFieldService;
        }

        readonly MultiTenantPlatformDbContext dbContext;
        readonly IMetaFieldService metaFieldService;

        public List<SearchConditionAggregation> GetListByInterfaceConditionId(int searchConditionId)
        {
            return dbContext.QueryList<SearchConditionAggregation>(t => t.SearchConditionId == searchConditionId);
        }

        public ResultModel Delete(int id)
        {
            dbContext.Delete<SearchConditionAggregation>(t => t.Id == id);
            return ResultModel.Success();
        }

        //组织接口搜索条件
        public ResultModel AggregateCondition(int interfaceConditionId, int brotherNodeId, int conditionJointTypeId, int fieldId, int conditionTypeId, string conditionValue, int conditionValueTypeId)
        {
            //如果不是参数传递值，则根据传入的字段校验数据
            if (conditionValueTypeId != (int)ConditionValueType.Parameter)
            {
                if (!metaFieldService.CheckAndGetFieldValueByFieldType(fieldId, conditionValue).IsSuccess)
                    return ResultModel.Error("字段值和字段定义的类型不匹配");
            }

            return TransactionHelper.Transaction(() =>
            {
                int parentId = brotherNodeId;
                //如果兄弟节点!=-1，说明当前树有值。反之，则构建新树
                if (parentId != -1)
                {
                    //判断是否有树存在
                    List<SearchConditionAggregation> conditionListExist = GetListByInterfaceConditionId(interfaceConditionId);
                    //查看当前兄弟节点的父节点id
                    SearchConditionAggregation brotherCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherNodeId);
                    parentId = brotherCondition.ParentId;
                    //拿到父节点的信息
                    SearchConditionAggregation parentCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherCondition.ParentId);
                    //如果父节点的连接条件和当前新建的条件一致，则不需要新建条件节点，直接附加在已有的条件下面
                    if (parentCondition == null || parentCondition.ConditionType != conditionJointTypeId)
                    {
                        //先添加一个父节点，然后把兄弟节点的父节点指向新建的父节点
                        string tempKey = DateTime.Now.ToString("yyyyMMddhhmmss");
                        SearchConditionAggregation newParentCondition = new SearchConditionAggregation
                        {
                            SearchConditionId = interfaceConditionId,
                            ParentId = conditionListExist.Count > 0 ? parentId : -1,//如果有树，则插入节点的父节点为刚才的兄弟节点的父节点，否则，插入-1作为根节点
                            FieldId = -1,//连接节点没有field
                            FieldCode = "-1",
                            FieldName = tempKey,
                            ConditionType = conditionJointTypeId,
                            Name = EnumsTranslaterUseInProgram.Tran_ConditionJoint(conditionJointTypeId),
                            Value = "-1",
                            ValueType = -1
                        };
                        base.Add(newParentCondition);
                        //查询刚才插入的节点
                        newParentCondition = dbContext.QueryOne<SearchConditionAggregation>(t => t.FieldName.Contains(tempKey));

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
                    if (dbContext.QueryExist<SearchConditionAggregation>(t => t.Id == parentId))
                    {
                        return ResultModel.Error("已经存在条件节点，请查证后操作！");
                    }
                }
                //新增节点
                MetaField metaField = metaFieldService.GetById(fieldId);
                SearchConditionAggregation newCondition = new SearchConditionAggregation
                {
                    SearchConditionId = interfaceConditionId,
                    ParentId = parentId,
                    FieldId = fieldId,
                    FieldName = metaField.Name,
                    FieldCode = metaField.Code,
                    ConditionType = conditionTypeId,
                    Name = $"{metaField.Name} {EnumsTranslaterUseInProgram.Tran_ConditionType(conditionTypeId)} {conditionValue}",
                    Value = conditionValue,
                    ValueType = conditionValueTypeId
                };
                base.Add(newCondition);

                return ResultModel.Success("保存成功！");
            });
        }

        //删除某个节点
        public ResultModel DeleteAggregateCondition(int nodeId, int searchConditionId)
        {
            //将要删除的节点id集合
            List<int> willBeDeleteIds = new List<int>();

            List<SearchConditionAggregation> allConditions = GetListByInterfaceConditionId(searchConditionId);
            if (allConditions == null || !allConditions.Any())
            {
                return ResultModel.Success("删除成功！");
            }
            SearchConditionAggregation conditionAggregation = allConditions.FirstOrDefault(t => t.Id == nodeId);
            if (conditionAggregation == null)
            {
                return ResultModel.Success("删除成功！");
            }
            //获取父节点id
            int parentId = conditionAggregation.ParentId;
            //如果是顶级节点，直接删除
            if (parentId == -1)
            {
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
                return ResultModel.Success("删除成功！");
            }
            //如果不是顶级节点，查询所有兄弟节点。
            //如果所有兄弟节点（包含自己）多余两个，则直接删除此节点;
            List<SearchConditionAggregation> conditionList = allConditions.Where(t => t.ParentId == parentId).ToList();
            if (conditionList.Count > 2)
            {
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
                return ResultModel.Success("删除成功！");
            }
            //如果兄弟节点为两个，则将父亲节点删除，将另一个兄弟节点作为父节点。
            else if (conditionList.Count == 2)
            {
                //获取到父节点
                SearchConditionAggregation parentNode = allConditions.FirstOrDefault(t => t.Id == parentId);
                //找到兄弟节点（同一个父节点，id!=当前节点）
                SearchConditionAggregation brotherNode = allConditions.FirstOrDefault(t => t.ParentId == parentId && t.Id != nodeId);
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
                return ResultModel.Success("删除成功！");
            }

            return ResultModel.Success("删除成功！");

            //删除节点级所有下级节点
            void DeleteNodeAndChildrenNodes(List<SearchConditionAggregation> sourceConditions, int currentNodeId)
            {
                FindDeleteNodeAndChildrenNodes(sourceConditions, currentNodeId);
                Expression<Func<SearchConditionAggregation, bool>> func = t => t.Id == nodeId;
                foreach (var item in willBeDeleteIds)
                {
                    func = func.Or(tt => tt.Id == item);
                }
                dbContext.Delete(func);
            }

            //遍历整棵树，找到要删除的节点以及下级节点
            void FindDeleteNodeAndChildrenNodes(List<SearchConditionAggregation> sourceConditions, int currentNodeId)
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
        /// <param name="searchConditionId">条件id</param>
        /// <param name="conditionValueDic">从http请求中传递过来的参数值集合</param>
        /// <returns></returns>
        public FilterDefinition<BsonDocument> AnalysisConditionToFilterDefinition(int metaObjectId, int searchConditionId, Dictionary<string, object> conditionValueDic)
        {
            var bf = Builders<BsonDocument>.Filter;
            //全部字段字典缓存
            Dictionary<int, MetaField> metaFieldIdDic = metaFieldService.GetMetaFieldDicIdObjUnDeleted(metaObjectId);

            //获取全部条件表达式
            List<SearchConditionAggregation> conditions = GetListByInterfaceConditionId(searchConditionId);
            if (conditions == null || !conditions.Any())
            {
                return null;
            }
            SearchConditionAggregation condition = conditions.FirstOrDefault(t => t.ParentId == -1);
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
            FilterDefinition<BsonDocument> ConditionRouter(SearchConditionAggregation routeCondition)
            {
                FilterDefinition<BsonDocument> filterDefinition = null;
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
                                        filterDefinition = bf.And(filterDefinition, ConditionRouter(item));
                                    }
                                    //如果是表达式语句
                                    else
                                    {
                                        filterDefinition = bf.And(filterDefinition, ConditionValue(item));
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
                                        filterDefinition = bf.Or(filterDefinition, ConditionRouter(item));
                                    }
                                    //如果是表达式语句
                                    else
                                    {
                                        filterDefinition = bf.Or(filterDefinition, ConditionValue(item));
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
            FilterDefinition<BsonDocument> ConditionValue(SearchConditionAggregation routeCondition)
            {
                //如果条件值来自参数,则从参数列表里面获取
                if (routeCondition.Value.Equals("?"))
                {
                    //从参数获取到值
                    string key = routeCondition.FieldCode;
                    var keyUpper = key.ToUpperInvariant();
                    //如果没有传递参数值，则抛出异常
                    if (!conditionValueDic.ContainsKey(keyUpper))
                    {
                        throw new ArgumentNullException(key, $"Conditions define field parameters [{key}] but do not provide values.");
                    }
                    object argumentValue = conditionValueDic.SafeGet(keyUpper);
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
                            return null;
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
                            return null;
                    }
                }
            }
        }
    }
}
