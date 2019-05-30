using SevenTiny.Bantina;

namespace Seventiny.Cloud.DataApi.Models
{
    public class QueryArgs
    {
        public string _interface { get; set; }
        public int _pageIndex { get; set; }
        public int _pageSize { get; set; }

        public Result QueryArgsCheck()
        {
            if (string.IsNullOrEmpty(_interface))
            {
                return Result.Error("interfaceCode can not be null!");
            }
            return Result.Success();
        }
    }
}
