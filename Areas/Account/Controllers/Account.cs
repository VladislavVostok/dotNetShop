using dotNetShop.Areas.Account.Models;
using dotNetShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotNetShop.Areas.Account.Controllers
{
    public class Account : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public Account(UserManager<ApplicationUser> manager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = manager;
            _signInManager = signInManager;

        }

        // /Account/Register
        [Route("{area}/{action}")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [Route("{area}/{action}")]
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            var user = model.GetUser();
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            { 
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
        }


        // /Account/Login
        [Route("{area}/{action}")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("{area}/{action}")]
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            var result = await _signInManager.PasswordSignInAsync(userName:  model.LogIn, password: model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return LocalRedirect("/");
            }
            else
            {
                ModelState.AddModelError("", $"Задан неверный логин или пароль");
            }
            return View("Login");
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("/");
        }

    }
}
