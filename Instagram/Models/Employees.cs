using Microsoft.AspNetCore.Identity;

namespace Instagram.Models;

public class Employees : IdentityUser<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Bio { get; set; }
    public string? Sex { get; set; }
}