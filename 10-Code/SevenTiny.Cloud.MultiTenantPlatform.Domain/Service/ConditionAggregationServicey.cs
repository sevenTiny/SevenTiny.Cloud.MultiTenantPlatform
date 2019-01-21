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
    public class ConditionAggregationService : Repository<ConditionAggregation>, IConditionAggregationService
    {
        public ConditionAggregationService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IMetaFieldService _metaFieldService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            metaFieldService = _metaFieldService;
        }

        readonly MultiTenantPlatformDbContext dbContext;
        readonly IMetaFieldService metaFieldService;

        public List<ConditionAggregation> GetByInterfaceConditionId(int interfaceSearchConditionId)
        {
            return dbContext.QueryList<ConditionAggregation>(t => t.InterfaceSearchConditionId == interfaceSearchConditionId);
        }

        public ResultModel Delete(int id)
        {
            dbContext.Delete<ConditionAggregation>(t => t.Id == id);
            return ResultModel.Success();
        }

        //组织接口搜索条件
        public ResultModel AggregateCondition(int interfaceConditionId, int brotherNodeId, int conditionJointTypeId, int fieldId, int conditionTypeId, string conditionValue, int conditionValueTypeId)
        {
            return TransactionHelper.Transaction(() =>
            {
                int parentId = brotherNodeId;
                //如果兄弟节点!=-1，说明当前树有值。反之，则构建新树
                if (parentId != -1)
                {
                    //判断是否有树存在
                    List<ConditionAggregation> conditionListExist = GetByInterfaceConditionId(interfaceConditionId);
                    //查看当前兄弟节点的父节点id
                    ConditionAggregation brotherCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherNodeId);
                    parentId = brotherCondition.ParentId;
                    //拿到父节点的信息
                    ConditionAggregation parentCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherCondition.ParentId);
                    //如果父节点的连接条件和当前新建的条件一致，则不需要新建条件节点，直接附加在已有的条件下面
                    if (parentCondition == null || parentCondition.ConditionType != conditionJointTypeId)
                    {
                        //先添加一个父节点，然后把兄弟节点的父节点指向新建的父节点
                        string tempKey = DateTime.Now.ToString("yyyyMMddhhmmss");
                        ConditionAggregation newParentCondition = new ConditionAggregation
                        {
                            InterfaceSearchConditionId = interfaceConditionId,
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
                        newParentCondition = dbContext.QueryOne<ConditionAggregation>(t => t.FieldName.Contains(tempKey));

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
                    if (dbContext.QueryExist<ConditionAggregation>(t => t.Id == parentId))
                    {
                        return ResultModel.Error("已经存在条件节点，请查证后操作！");
                    }
                }
                //新增节点
                MetaField metaField = metaFieldService.GetById(fieldId);
                ConditionAggregation newCondition = new ConditionAggregation
                {
                    InterfaceSearchConditionId = interfaceConditionId,
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
        public ResultModel DeleteAggregateCondition(int nodeId, int interfaceSearchConditionId)
        {
            //将要删除的节点id集合
            List<int> willBeDeleteIds = new List<int>();

            List<ConditionAggregation> allConditions = GetByInterfaceConditionId(interfaceSearchConditionId);
            if (allConditions == null || !allConditions.Any())
            {
                return ResultModel.Success("删除成功！");
            }
            ConditionAggregation conditionAggregation = allConditions.FirstOrDefault(t => t.Id == nodeId);
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
            List<ConditionAggregation> conditionList = allConditions.Where(t => t.ParentId == parentId).ToList();
            if (conditionList.Count > 2)
            {
                DeleteNodeAndChildrenNodes(allConditions, nodeId);
                return ResultModel.Success("删除成功！");
            }
            //如果兄弟节点为两个，则将父亲节点删除，将另一个兄弟节点作为父节点。
            else if (conditionList.Count == 2)
            {
                //获取到父节点
                ConditionAggregation parentNode = allConditions.FirstOrDefault(t => t.Id == parentId);
                //找到兄弟节点（同一个父节点，id!=当前节点）
                ConditionAggregation brotherNode = allConditions.FirstOrDefault(t => t.ParentId == parentId && t.Id != nodeId);
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
            void DeleteNodeAndChildrenNodes(List<ConditionAggregation> sourceConditions, int currentNodeId)
            {
                FindDeleteNodeAndChildrenNodes(sourceConditions, currentNodeId);
                Expression<Func<ConditionAggregation, bool>> func = t => t.Id == nodeId;
                foreach (var item in willBeDeleteIds)
                {
                    func = func.Or(tt => tt.Id == item);
                }
                dbContext.Delete(func);
            }

            //遍历整棵树，找到要删除的节点以及下级节点
            void FindDeleteNodeAndChildrenNodes(List<ConditionAggregation> sourceConditions, int currentNodeId)
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
    }
}
