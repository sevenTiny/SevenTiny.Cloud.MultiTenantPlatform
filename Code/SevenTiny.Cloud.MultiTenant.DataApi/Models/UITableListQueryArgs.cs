using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    public class UITableListQueryArgs
    {
        /// <summary>
        /// 视图编码
        /// </summary>
        public string ViewName { get; set; }
        /// <summary>
        /// 对象编码
        /// </summary>
        public string MetaObject { get; set; }
        /// <summary>
        /// 应用编码
        /// </summary>
        public string Application { get; set; }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public SearchData SearchData { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public SortField[] SortFields { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 参数校验
        /// </summary>
        /// <returns></returns>
        public Result ArgsCheck()
        {
            if (string.IsNullOrEmpty(ViewName))
            {
                return Result.Error("ViewName can not be null!");
            }
            return Result.Success();
        }
    }
}
