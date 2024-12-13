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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PublicationCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            Employees user = await _userManager.GetUserAsync(User);
            if (model.Image != null && model.Image.Length > 0 && model.Image.ContentType.StartsWith("image/"))
            {
                string fileName = $"publication_{user.Email}_{Guid.NewGuid().ToString()}{Path.GetExtension(model.Image.FileName)}";
                string filePath = Path.Combine(_environment.WebRootPath, "publications", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                Publication publication = new Publication()
                {
                    ImageUrl = $"/publications/{fileName}",
                    Description = model.Description,
                    UserId = user.Id,
                    DateOfCreation = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Publications.Add(publication);
                user.PostCount++;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Image", "Можно загрузить только изображения(фото).");
            }
        }

        string? returnUrl = Request.Headers["Referer"].ToString();
        if (!string.IsNullOrEmpty(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Publication? publication = await _context.Publications.FindAsync(id);
        Employees currentUser = await _userManager.GetUserAsync(User);

        if (publication != null)
        {
            if (currentUser.Id != publication.UserId)
            {
                return BadRequest();
            }

            _context.Publications.Remove(publication);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}