using Microsoft.AspNetCore.Identity;

namespace Instagram.Models;

public class Employees : IdentityUser<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Bio { get; set; }
    public string? Sex { get; set; }
    
    public int PostCount { get; set; }        // количесво публикаций пользователя
    public int FollowingCount { get; set; }  // количество подписок пользователя
    public int FollowerCount { get; set; }  // количество подписчиков пользователя
}