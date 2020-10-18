using System.ComponentModel.DataAnnotations;

namespace MonitoringApp.ViewModels
{
    public class UserModelForResetPassword
    {
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
