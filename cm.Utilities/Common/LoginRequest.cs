namespace cm.Utilities.Common
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public object ExtraProps { get; set; }
    }
}