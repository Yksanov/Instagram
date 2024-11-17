using Instagram.Models;
using Instagram.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Instagram.Controllers;

public class PublicationController : Controller
{
    private readonly InstagramContext _context;
    private readonly UserManager<Employees> _userManager;
    private readonly IWebHostEnvironment _environment;

    public PublicationController(InstagramContext context, UserManager<Employees> userManager, IWebHostEnvironment environment)
    {
        _context = context;
        _userManager = userManager;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        Employees currentUser = await _userManager.GetUserAsync(User);
        IQueryable<Publication> subscribedsPublications = _context.Publications.Where(p => p.UserId == currentUser.Id || p.UserId == currentUser.Id);
        
        List<Publication> publications = await subscribedsPublications
            .Include(p => p.User)
            .OrderBy(p => p.DateOfCreation)
            .ToListAsync();
        
        publications.Reverse();

        return View(new IndexViewModels()
        {
            CurrentUser = currentUser,
            Publications = publications
        });
    }
}