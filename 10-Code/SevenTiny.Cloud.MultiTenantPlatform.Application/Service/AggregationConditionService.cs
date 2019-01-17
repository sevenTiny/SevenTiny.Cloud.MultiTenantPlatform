//using MongoDB.Bson;
//using MongoDB.Driver;
//using SevenTiny.Bantina.Extensions;
//using SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
//using System.Collections.Generic;
//using System.Linq;

//namespace SevenTiny.Cloud.MultiTenantPlatform.Application.Service
//{
//    public class AggregationConditionService : IAggregationConditionService
//    {
//        private readonly IConditionAggregationRepository _conditionAggregationRepository;
//        public AggregationConditionService(IConditionAggregationRepository conditionAggregationRepository)
//        {
//            this._conditionAggregationRepository = conditionAggregationRepository;
//        }

//        /// <summary>
//        /// 将条件配置解析成mongodb可以执行的条件
//        /// </summary>
//        /// <param name="interfaceSearchConditionId">条件id</param>
//        /// <param name="conditionValueDic">从http请求中传递过来的参数值集合</param>
//        /// <returns></returns>
//        public FilterDefinition<BsonDocument> AnalysisConditionToFilterDefinition(int interfaceSearchConditionId, Dictionary<string, object> conditionValueDic)
//        {
//            var bf = Builders<BsonDocument>.Filter;

//            //获取全部条件表达式
//            List<ConditionAggregation> conditions = _conditionAggregationRepository.GetList(t => t.InterfaceSearchConditionId == interfaceSearchConditionId);
//            if (conditions == null || !conditions.Any())
//            {
//                return null;
//            }
//            ConditionAggregation condition = conditions.FirstOrDefault(t => t.ParentId == -1);
//            if (condition == null)
//            {
//                return null;
//            }

//            //如果valueType==-1，则表明这个是连接条件
//            if (condition.ValueType == -1)
//            {
//                //通过链接条件解析器进行解析
//                return ConditionRouter(condition);
//            }
//            //valueType!=-1 则表明这是个语句
//            else
//            {
//                //通过条件表达式语句解析器解析
//                return ConditionValue(condition);
//            }

//            //连接条件解析器。如果是连接条件， 则执行下面逻辑将左...右子条件解析
//            FilterDefinition<BsonDocument> ConditionRouter(ConditionAggregation routeCondition)
//            {
//                FilterDefinition<BsonDocument> filterDefinition = null;
//                //将子节点全部取出
//                var routeConditionChildren = conditions.Where(t => t.ParentId == routeCondition.Id).ToList();
//                var first = routeConditionChildren.FirstOrDefault();
//                if (first != null)
//                {
//                    //如果字节点是连接条件
//                    if (first.ValueType == -1)
//                    {
//                        filterDefinition = ConditionRouter(first);
//                    }
//                    //如果是语句
//                    else
//                    {
//                        filterDefinition = ConditionValue(first);
//                        //根据根节点的连接条件执行不同的连接操作
//                        switch (routeCondition.ConditionType)
//                        {
//                            case (int)ConditionJoint.And:
//                                //子节点全部是与逻辑
//                                foreach (var item in routeConditionChildren.Except(new[] { first }))
//                                {
//                                    //如果valueType==-1 则表明是连接条件
//                                    if (item.ValueType == -1)
//                                    {
//                                        filterDefinition = bf.And(filterDefinition, ConditionRouter(item));
//                                    }
//                                    //如果是表达式语句
//                                    else
//                                    {
//                                        filterDefinition = bf.And(filterDefinition, ConditionValue(item));
//                                    }
//                                }
//                                break;
//                            case (int)ConditionJoint.Or:
//                                //子节点全部是或逻辑
//                                foreach (var item in routeConditionChildren.Except(new[] { first }))
//                                {
//                                    //如果valueType==-1 则表明是连接条件
//                                    if (item.ValueType == -1)
//                                    {
//                                        filterDefinition = bf.Or(filterDefinition, ConditionRouter(item));
//                                    }
//                                    //如果是表达式语句
//                                    else
//                                    {
//                                        filterDefinition = bf.Or(filterDefinition, ConditionValue(item));
//                                    }
//                                }
//                                break;
//                            default:
//                                return null;
//                        }
//                    }
//                    return filterDefinition;
//                }
//                return null;
//            }

//            //条件值解析器
//            FilterDefinition<BsonDocument> ConditionValue(ConditionAggregation routeCondition)
//            {
//                //如果条件值来自参数,则从参数列表里面获取
//                if (routeCondition.Value.Equals("?"))
//                {
//                    switch (routeCondition.ConditionType)
//                    {
//                        case (int)ConditionType.Equal:
//                            return bf.Eq(routeCondition.FieldCode, conditionValueDic.SafeGet(routeCondition.FieldCode));
//                        case (int)ConditionType.GreaterThan:
//                            return bf.Gt(routeCondition.FieldCode, conditionValueDic.SafeGet(routeCondition.FieldCode));
//                        case (int)ConditionType.GreaterThanEqual:
//                            return bf.Gte(routeCondition.FieldCode, conditionValueDic.SafeGet(routeCondition.FieldCode));
//                        case (int)ConditionType.LessThan:
//                            return bf.Lt(routeCondition.FieldCode, conditionValueDic.SafeGet(routeCondition.FieldCode));
//                        case (int)ConditionType.LessThanEqual:
//                            return bf.Lte(routeCondition.FieldCode, conditionValueDic.SafeGet(routeCondition.FieldCode));
//                        case (int)ConditionType.NotEqual:
//                            return bf.Ne(routeCondition.FieldCode, conditionValueDic.SafeGet(routeCondition.FieldCode));
//                        default:
//                            return null;
//                    }
//                }
//                //如果来自配置，则直接从配置里面获取到值
//                else
//                {
//                    switch (routeCondition.ConditionType)
//                    {
//                        case (int)ConditionType.Equal:
//                            return bf.Eq(routeCondition.FieldCode, routeCondition.Value);
//                        case (int)ConditionType.GreaterThan:
//                            return bf.Gt(routeCondition.FieldCode, routeCondition.Value);
//                        case (int)ConditionType.GreaterThanEqual:
//                            return bf.Gte(routeCondition.FieldCode, routeCondition.Value);
//                        case (int)ConditionType.LessThan:
//                            return bf.Lt(routeCondition.FieldCode, routeCondition.Value);
//                        case (int)ConditionType.LessThanEqual:
//                            return bf.Lte(routeCondition.FieldCode, routeCondition.Value);
//                        case (int)ConditionType.NotEqual:
//                            return bf.Ne(routeCondition.FieldCode, routeCondition.Value);
//                        default:
//                            return null;
//                    }

//                }
//            }
//        }
//    }
//}
