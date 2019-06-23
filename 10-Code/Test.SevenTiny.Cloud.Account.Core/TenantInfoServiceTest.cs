using SevenTiny.Cloud.Account.Core.DataAccess;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Enum;
using SevenTiny.Cloud.Account.Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test.SevenTiny.Cloud.Account.Core
{
    public class TenantInfoServiceTest
    {
        [Fact]
        public void AddTenantInfo()
        {
            var tenantInfoService = new TenantInfoService(new AccountDbContext());
            var result = tenantInfoService.Add(new TenantInfo
            {
                OperatorName = "7tiny",
                IsActive = (int)TrueFalse.True,
                Description = "测试租户",
                RegisterEmail = "seventiny@foxmail.com",
                TenantName = "SevenTiny测试租户"
            });

            Assert.True(result.IsSuccess);
        }
    }
}
