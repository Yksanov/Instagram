using System.ComponentModel.DataAnnotations;
using Instagram.Models;

namespace Instagram.ViewModel;

public class PublicationCreateViewModel
{
    public IFormFile Image { get; set; }
    [StringLength(300, ErrorMessage = "Описание не может быть больше 300 символов.")]
    public string Description { get; set; }
}