using FinalProject.Models;
using FinalProject.ViewModels.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [Authorize]

    public class BasketController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public BasketController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> AddBasket(BasketAddVM basketAddVM)
        {
            if (!ModelState.IsValid) return BadRequest(basketAddVM);
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            return View();
        }
    }
}
