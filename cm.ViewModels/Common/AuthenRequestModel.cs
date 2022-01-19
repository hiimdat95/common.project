namespace cm.ViewModels.Common
{
    public class AuthenRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RemoteIpAddress { get; set; }
        public object ExtraProps { get; set; }
    }
}