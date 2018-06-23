using SevenTiny.Bantina.Extensions;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application
{
    public class AggregationConditionService : IAggregationConditionService
    {
        private readonly IConditionAggregationRepository _conditionAggregationRepository;
        public AggregationConditionService(IConditionAggregationRepository conditionAggregationRepository)
        {
            this._conditionAggregationRepository = conditionAggregationRepository;
        }

        public Expression<Func<ConditionAggregation, bool>> AnalysisConditionToLambda(int interfaceSearchConditionId,Dictionary<string,object> conditionValueDic)
        {
            return null;

            //获取全部条件表达式
            List<ConditionAggregation> conditions = _conditionAggregationRepository.GetList(t => t.InterfaceSearchConditionId == interfaceSearchConditionId);
            if (conditions == null || !conditions.Any())
            {
                return t => false;
            }
            ConditionAggregation condition = conditions.FirstOrDefault(t => t.ParentId == -1);
            if (condition == null)
            {
                return t => false;
            }

            //如果valueType==-1，则表明这个是连接条件
            if (condition.ValueType == -1)
            {
                if (condition.ConditionType == (int)ConditionJoint.And)
                {
                    Expression<Func<ConditionAggregation, bool>> and = t => true;
                    //查询出所有子条件
                    List<ConditionAggregation> andConditions = conditions.Where(t => t.ParentId == condition.Id).ToList();
                    foreach (var item in andConditions)
                    {
                        //如果是连接条件
                        if (item.ValueType == -1)
                        {

                        }
                        //如果是表达式，则直接拼接到lambda表达式里面
                        else
                        {
                            //and = and.And()
                        }
                    }
                }
                else if (condition.ConditionType == (int)ConditionJoint.Or)
                {

                }
            }

            Expression<Func<ConditionAggregation, bool>> AnalysisConditionType(int field,int conditionTypeId,object value)
            {
                return null;
            }


            condition.Children = GetTree(conditions, condition.Id);

            //Tree Search
            List<ConditionAggregation> GetTree(List<ConditionAggregation> source, int parentId)
            {
                var childs = source.Where(t => t.ParentId == parentId).ToList();
                if (childs == null)
                {
                    return new List<ConditionAggregation>();
                }
                else
                {
                    childs.ForEach(t => t.Children = GetTree(source, t.Id));
                }
                return childs;
            }
        }
    }
}
