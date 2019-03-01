using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;

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
        public ResultModel ArgsCheck()
        {
            if (string.IsNullOrEmpty(ViewName))
            {
                return ResultModel.Error("ViewName can not be null!");
            }
            return ResultModel.Success();
        }
    }
}
