namespace ViewModels.Auth
{
    public class RefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string RemoteIpAdderss { get; set; }
    }
}