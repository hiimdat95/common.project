using System.ComponentModel.DataAnnotations;

namespace ViewModels.Auth
{
    public class ValidateTokenModel
    {
        [Required]
        public string Token { get; set; }
    }
}