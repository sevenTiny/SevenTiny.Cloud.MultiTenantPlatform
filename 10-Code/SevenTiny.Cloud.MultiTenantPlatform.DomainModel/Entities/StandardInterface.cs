using SevenTiny.Cloud.MultiTenantPlatform.Model.Enums;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    public class StandardInterface : EntityInfo
    {
        public InterfaceType InterfaceType { get; set; }
        public int SearchFormId { get; set; }
        public int TableListId { get; set; }
        public int FormId { get; set; }
        /// <summary>
        /// data source of enume id
        /// </summary>
        public int EnumDataSourceId { get; set; }
    }
}
