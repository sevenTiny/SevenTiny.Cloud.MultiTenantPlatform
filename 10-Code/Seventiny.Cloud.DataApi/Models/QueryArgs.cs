using SevenTiny.Bantina;

namespace Seventiny.Cloud.DataApi.Models
{
    public class QueryArgs
    {
        public string _interfaceCode { get; set; }
        public int _pageIndex { get; set; }
        public int _pageSize { get; set; }

        public Result QueryArgsCheck()
        {
            if (string.IsNullOrEmpty(_interfaceCode))
            {
                return Result.Error("interfaceCode can not be null!");
            }
            return Result.Success();
        }
    }
}
