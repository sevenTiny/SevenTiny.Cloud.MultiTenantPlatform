using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    public class Entity: EntityInfo
    {
        public int ApplicationId { get; set; }
        /// <summary>
        /// Foreign table
        /// </summary>
        public MetaField[] MetaFields { get; set; }
    }
}
