using FinalProject.Helpers.Enums;
using FinalProject.Models;
using FinalProject.Services.Interfaces;
using FinalProject.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            AppUser user = new AppUser
            {
                Fullname = registerVM.Fullname,
                UserName = registerVM.Username,
                Email = registerVM.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                   
                }
                return View(registerVM);
            }

            //await _signInManager.SignInAsync(user, false);
            return RedirectToAction(nameof(VerifyEmail));

        }

        public IActionResult VerifyEmail()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            AppUser user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
           
            if(user is null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            }

            if(user is null)
            {
                ModelState.AddModelError("", "Email or password wrong");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
           
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password wrong");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}
