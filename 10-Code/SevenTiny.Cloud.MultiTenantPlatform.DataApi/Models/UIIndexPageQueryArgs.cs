using SevenTiny.Bantina;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    public class UIIndexPageQueryArgs
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
