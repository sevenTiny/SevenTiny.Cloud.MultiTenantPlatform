using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class MenueService : MetaObjectCommonServiceBase<Menue>, IMenueService
    {
        public MenueService(IMenueRepository menueRepository) : base(menueRepository)
        {
        }

        public new Result Update(Menue entity)
        {
            return base.UpdateWithOutCode(entity, target => {
                target.Icon = entity.Icon;
                target.LinkType = entity.LinkType;
                target.Address = entity.Address;
            });
        }
    }
}
