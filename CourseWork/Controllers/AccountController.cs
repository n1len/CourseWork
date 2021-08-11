using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CourseWork.Data;
using CourseWork.Infrastructure.Models;
using CourseWork.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.UserName, Email = model.Email, IsBlocked = false};
                
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.UserName, model.Password,false,false);
                if (result.Succeeded)
                {
                    if((await _userManager.FindByNameAsync(model.UserName)).IsBlocked != true) 
                        return RedirectToAction("Index", "Home");
                    else
                        ModelState.AddModelError("", "Аккаунт заблокирован");
                }
                else
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Personal()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userId = currentUser.Id;

            var user = await _context.Users
                .Include(u => u.CustomCollections)
                .FirstOrDefaultAsync(i => i.Id == userId);

            return View(user);
        }
    }
}
