using System.ComponentModel.DataAnnotations;

namespace Instagram.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Display(Name = "Запомнить?")]
    public bool RememberMe { get; set; }
    
    public string ReturnUrl { get; set; }
}