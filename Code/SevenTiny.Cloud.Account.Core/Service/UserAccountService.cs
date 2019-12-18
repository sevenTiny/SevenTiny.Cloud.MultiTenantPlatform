using SevenTiny.Bantina;
using SevenTiny.Bantina.Security;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.Account.Core.DataAccess;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Enum;
using SevenTiny.Cloud.Account.Core.Repository;
using SevenTiny.Cloud.Account.Core.ServiceContract;
using SevenTiny.Cloud.Infrastructure.Const;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.Account.Core.Service
{
    public class UserAccountService : CommonInfoRepository<UserAccount>, IUserAccountService
    {
        AccountDbContext _dbContext;
        public UserAccountService(AccountDbContext accountDbContext) : base(accountDbContext)
        {
            _dbContext = accountDbContext;
        }

        public new Result<UserAccount> Update(UserAccount entity)
        {
            UserAccount old = GetById(entity.Id);
            if (old != null)
            {
                //编码不允许修改
                old.HasOfficialSystemPermission = entity.HasOfficialSystemPermission;
                old.HasSettingSystemPermission = entity.HasSettingSystemPermission;
                //如果属于普通权限的才可以赋值，不允许直接给租户管理员
                if (SystemIdentityProvider.Collection().Contains(entity.SystemIdentity))
                {
                    old.SystemIdentity = entity.SystemIdentity;
                }
                old.Phone = entity.Phone;

                old.Name = entity.Name;
                old.Group = entity.Group;
                old.SortNumber = entity.SortNumber;
                old.Description = entity.Description;
                old.ModifyBy = entity.ModifyBy;
                old.ModifyTime = DateTime.Now;
            }
            return base.Update(old);
        }

        /// <summary>
        /// 校验注册信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Result ValidateRegisterdByEmail(string email)
        {
            return Result.Success()
                   .ContinueAssert(!string.IsNullOrEmpty(email), "邮箱信息为空")
                   .Continue(re =>
                   {
                       bool isExist = _dbContext.Queryable<UserAccount>().Where(t => t.Email == email).Any();
                       //isExist为false，说明未注册
                       return re.ContinueAssert(isExist == false, "已存在该用户的注册信息");
                   });
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public Result SignUpByEmail(UserAccount userAccount)
        {
            return Result.Success("Register Succeed!")
                .ContinueAssert(userAccount.TenantId >= 0, "非法的租户")
                .ContinueAssert(!string.IsNullOrEmpty(userAccount.Email), "邮箱不能为空")
                .ContinueAssert(userAccount.Email.IsEmail(), "邮箱格式不正确")
                //check register
                .Continue(re => ValidateRegisterdByEmail(userAccount.Email))
                .Continue(re =>
                {
                    //如果属于普通权限的才可以赋值，不允许直接给租户管理员
                    if (SystemIdentityProvider.Collection().Contains(userAccount.SystemIdentity))
                    {
                        return Result.Error("非法授权");
                    }
                    return re;
                })
                //+salt
                .Continue(re =>
                {
                    userAccount.Password = GetSaltPwd(userAccount.Password);
                    return re;
                })
                //register
                .Continue(re =>
                {
                    return Add(userAccount);
                });
        }

        /// <summary>
        /// 登陆并返回数据库中的账号信息
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public Result<UserAccount> LoginByEmail(UserAccount userAccount)
        {
            return Result<UserAccount>.Success("登陆成功")
                .ContinueAssert(!string.IsNullOrEmpty(userAccount.Email), "邮箱不能为空")
                .ContinueAssert(!string.IsNullOrEmpty(userAccount.Password), "密码不能为空")
                .ContinueAssert(userAccount.Email.IsEmail(), "邮箱格式不正确")
                .ContinueAssert(userAccount.Password.Length >= 6, "密码不能少于6位")
                .Continue(re =>
                {
                    UserAccount existAccount = _dbContext.Queryable<UserAccount>().Where(t => t.Email.Equals(userAccount.Email)).ToOne();
                    //返回的数据是查询到的数据
                    re.Data = existAccount;
                    return re.ContinueAssert(existAccount != null, "账号不存在")
                            .Continue(ree =>
                            {
                                if (!existAccount.Password.Equals(GetSaltPwd(userAccount.Password)))
                                    return Result<UserAccount>.Error("密码错误");
                                return ree;
                            });
                });
        }

        /// <summary>
        /// 为密码加盐
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private string GetSaltPwd(string pwd)
        {
            //为降低暴力破解的可能，密码强制前后加盐
            return MD5Helper.GetMd5Hash(string.Concat(AccountConst.SALT_BEFORE, pwd, AccountConst.SALT_AFTER));
        }

        public Result<List<UserAccount>> GetUserAccountsByTenantId(int tenantId)
        {
            return Result<List<UserAccount>>.Success("获取列表成功")
                .ContinueAssert(tenantId >= 0, "非法的租户")
                .Continue(re =>
                {
                    re.Data = _dbContext.Queryable<UserAccount>().Where(t => t.TenantId == tenantId && t.IsDeleted == (int)IsDeleted.UnDeleted).ToList();
                    return re;
                });
        }
    }
}
