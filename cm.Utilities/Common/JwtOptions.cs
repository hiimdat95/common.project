namespace cm.Utilities.Common
{
    public class JwtOptions
    {
        public int ExpiresInMinutes { get; set; }
        public string Secret { get; set; }
    }
}