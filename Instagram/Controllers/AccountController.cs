using Instagram.Models;
using Instagram.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<Employees> _userManager;
    private readonly SignInManager<Employees> _signInManager;
    private readonly IWebHostEnvironment _environment;

    public AccountController(UserManager<Employees> userManager, SignInManager<Employees> signInManager, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _environment = environment;
    }

    [HttpGet]
    public IActionResult Index(string returnUrl = "")
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }
}