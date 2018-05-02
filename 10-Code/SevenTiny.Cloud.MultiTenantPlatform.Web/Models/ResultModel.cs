namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Models
{
    public class ActionResultModel
    {
        public ActionResultModel(bool isSuccess, string msg)
        {
            IsSuccess = isSuccess;
            Msg = msg;
        }

        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
    }

    public class ActionResultModel<T1>
    {
        public ActionResultModel(bool isSuccess, string msg, T1 data)
        {
            IsSuccess = isSuccess;
            Msg = msg;
            Data = data;
        }

        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public T1 Data { get; set; }
    }

    public class ActionResultModel<T1, T2>
    {
        public ActionResultModel(bool isSuccess, string msg, T1 data, T2 data2)
        {
            IsSuccess = isSuccess;
            Msg = msg;
            Data = data;
            Data2 = data2;
        }

        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public T1 Data { get; set; }
        public T2 Data2 { get; set; }
    }

    public class ActionResultModel<T1, T2, T3>
    {
        public ActionResultModel(bool isSuccess, string msg, T1 data, T2 data2, T3 data3)
        {
            IsSuccess = isSuccess;
            Msg = msg;
            Data = data;
            Data2 = data2;
            Data3 = data3;
        }

        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public T1 Data { get; set; }
        public T2 Data2 { get; set; }
        public T3 Data3 { get; set; }
    }

    public class ActionResultModel<T1, T2, T3, T4>
    {
        public ActionResultModel(bool isSuccess, string msg, T1 data, T2 data2, T3 data3, T4 data4)
        {
            IsSuccess = isSuccess;
            Msg = msg;
            Data = data;
            Data2 = data2;
            Data3 = data3;
            Data4 = data4;
        }

        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public T1 Data { get; set; }
        public T2 Data2 { get; set; }
        public T3 Data3 { get; set; }
        public T4 Data4 { get; set; }
    }

    public class ActionResultModel<T1, T2, T3, T4, T5>
    {
        public ActionResultModel(bool isSuccess, string msg, T1 data, T2 data2, T3 data3, T4 data4, T5 data5)
        {
            IsSuccess = isSuccess;
            Msg = msg;
            Data = data;
            Data2 = data2;
            Data3 = data3;
            Data4 = data4;
            Data5 = data5;
        }

        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public T1 Data { get; set; }
        public T2 Data2 { get; set; }
        public T3 Data3 { get; set; }
        public T4 Data4 { get; set; }
        public T5 Data5 { get; set; }
    }
}
