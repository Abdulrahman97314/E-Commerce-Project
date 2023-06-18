using AdminPanal.Helpers;
using AdminPanal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Core.Entities.Identity;

namespace AdminPanal.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [RequireAnonymous]

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = await userManager.FindByEmailAsync(login.Email);
            if (user == null) {
                ModelState.AddModelError("Email", "Email is invalid");
                return View(login);
            }
            var result = await signInManager.PasswordSignInAsync(user, login.Password,true,false);
            if (!result.Succeeded || !await userManager.IsInRoleAsync(user, "Admin")) 
            {
                ModelState.AddModelError("Password", "You Are Not authorized");
                return View(login);
            }
             await userManager.UpdateSecurityStampAsync(user);

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login"); 
        }
    }
}
