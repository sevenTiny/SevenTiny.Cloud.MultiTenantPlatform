using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Model.Entities
{
    public class Entity: EntityInfo
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Foreign table
        /// </summary>
        public MetaField[] MetaFields { get; set; }
    }
}
