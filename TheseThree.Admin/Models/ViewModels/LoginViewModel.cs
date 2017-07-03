using System.ComponentModel.DataAnnotations;

namespace TheseThree.Admin.Models.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "用户名")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}