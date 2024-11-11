using System.ComponentModel.DataAnnotations;

namespace Instagram.ViewModel;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Login is required")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Format gmail is not correct")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Avatar is required")]
    public IFormFile Avatar { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
    
    [StringLength(20, ErrorMessage = "Максимальная длина имени 20 символов")]
    public string? Name { get; set; }
    
    [MaxLength(200, ErrorMessage = "Длина информации о пользователе не можеть превышать 200 символов")]
    public string? Bio { get; set; }
    
    [StringLength(20, ErrorMessage = "Максимальная длина имени 20 символов")]
    public string? PhoneNumber { get; set; }
    
    public string? Gender { get; set; }
}