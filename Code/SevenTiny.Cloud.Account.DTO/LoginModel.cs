namespace SevenTiny.Cloud.Account.DTO
{
    public class LoginModel
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 跳转的地址
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}
