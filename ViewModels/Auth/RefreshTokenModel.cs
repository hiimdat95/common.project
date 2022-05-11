using System.ComponentModel.DataAnnotations;

namespace ViewModels.Auth
{
    public class RefreshTokenModel
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}