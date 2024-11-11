using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Instagram.Models;

public class InstagramContext : IdentityDbContext<Employees, IdentityRole<int>, int>
{
    public DbSet<Employees> Employees { get; set; }

    public InstagramContext(DbContextOptions<InstagramContext> options) : base(options) {}
}