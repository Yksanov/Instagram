using Instagram.Models;
using Instagram.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            Employees user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return RedirectToAction("Index"); // надо изменить!!
            }
            
            ModelState.AddModelError(string.Empty, "Неправильное логин или пароль");
        }

        return View(model);
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            string filename = $"avatar_{model.Email}{Path.GetExtension(model.Avatar.FileName)}";

            if (model.Avatar != null && model.Avatar.Length > 0 && model.Avatar.ContentType.StartsWith("image/"))
            {
                string filePath = Path.Combine(_environment.WebRootPath, "avatars", filename);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Avatar.CopyToAsync(stream);
                }
            }
            else
            {
                ModelState.AddModelError("Avatar", "Изображение может быть только картинкой(image)");
                return View(model);
            }

            Employees user = new Employees()
            {
                Email = model.Email,
                UserName = model.UserName.ToLower(),
                Name = model.Name,
                Bio = model.Bio,
                PhoneNumber = model.PhoneNumber,
                Sex = model.Gender
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Account");  // надо изменить
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }
}