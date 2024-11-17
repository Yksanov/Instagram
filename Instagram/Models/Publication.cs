namespace Instagram.Models;

public class Publication
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public DateOnly DateOfCreation { get; set; }
    public int UserId { get; set; }
    public Employees User { get; set; }
}