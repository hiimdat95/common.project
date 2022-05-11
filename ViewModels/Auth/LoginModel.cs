using System.ComponentModel.DataAnnotations;

namespace ViewModels.Auth
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public object ExtraProps { get; set; }
    }
}