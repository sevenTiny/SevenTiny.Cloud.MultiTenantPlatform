using SevenTiny.Cloud.MultiTenantPlatform.Application.Service;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test.SevenTiny.Cloud.MultiTenantPlatform.UnitTest.Application
{
    public class AggregationConditionServiceTest
    {
        [Fact]
        public void AnalysisConditionToFilterDefinitionTest()
        {
            var conditionValueDic = new Dictionary<string, object>();
            var instance = new AggregationConditionService(new ConditionAggregationRepository(new MultiTenantPlatformDbContext()));
            var result = instance.AnalysisConditionToFilterDefinition(136, conditionValueDic);
        }
    }
}
