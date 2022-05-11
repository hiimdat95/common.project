using System.ComponentModel.DataAnnotations;

namespace ViewModels.Auth
{
    public class ChangePassReuqestModel
    {
        public string PasswordOld { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }

    public class ForgotPasswordRequestModel
    {
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ResetPasswordRequestModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}