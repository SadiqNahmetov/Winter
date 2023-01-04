using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _context;
        public BasketController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
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

            var product = await _context.Products.FindAsync(basketAddVM.Id);
            if (product == null) return NotFound();

            var basket = await _context.Baskets.FirstOrDefaultAsync(m => m.AppUserId == user.Id);
            if(basket == null)
            {
                basket = new Basket
                {
                    AppUserId = user.Id
                };
                 await _context.Baskets.AddAsync(basket);

                 await _context.SaveChangesAsync();

            }

            var basketProduct = new BasketProduct
            {
                BasketId = basket.Id,
                ProductId = product.Id,
                Quantity = 1
            };

            await _context.BasketProducts.AddAsync(basketProduct);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
