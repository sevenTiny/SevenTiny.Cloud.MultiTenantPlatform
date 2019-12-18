using SevenTiny.Bantina.Security;
using SevenTiny.Cloud.Infrastructure.Const;
using SevenTiny.Cloud.Account.Core.DataAccess;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Enum;
using SevenTiny.Cloud.Account.Core.Service;
using System;
using Xunit;

namespace Test.SevenTiny.Cloud.Account.Core
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void GeneratePassword()
        {
            //为降低暴力破解的可能，密码强制前后加盐
            string password = "123456";
            var pwd = MD5Helper.GetMd5Hash(string.Concat(AccountConst.SALT_BEFORE, password, AccountConst.SALT_AFTER)); ;
        }

        [Fact]
        public void AddUserAccount()
        {
            var service = new UserAccountService(new AccountDbContext());
            var result = service.Add(new UserAccount
            {
                TenantId = 100000,
                Name = "7tiny",
                Email = "seventiny@foxmail.com",
                Phone = "130110110110",
                Password = MD5Helper.GetMd5Hash(string.Concat(AccountConst.SALT_BEFORE, "123456", AccountConst.SALT_AFTER))
            });

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void GetUser()
        {
            var _dbContext = new AccountDbContext();
            var result = _dbContext.Queryable<UserAccount>().Where(t=>t.Email.Equals("seventiny@foxmail.com")).ToList();
        }
    }
}
