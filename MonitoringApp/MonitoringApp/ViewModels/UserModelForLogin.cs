using System.ComponentModel.DataAnnotations;

namespace MonitoringApp.ViewModels
{
    public class UserModelForLogin
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
