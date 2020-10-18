using System.ComponentModel.DataAnnotations;

namespace MonitoringApp.ViewModels
{
    public class UserModelForChangePassword
    {
        [Required(ErrorMessage = "Eski şifrenizi belirtmelisiniz")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifrenizi belirtmelisiniz")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Yeni şifreler uyuşmuyor")]
        public string ConfirmNewPassword { get; set; }
    }
}
