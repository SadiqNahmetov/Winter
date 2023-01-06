using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public ProductDetailController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int? id)
        {

            Product product = await _context.Products
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.ProductImages)
                .Include(m => m.Category)
                .Include(m => m.Brand)
                .Include(m => m.ProductSizes)
                .FirstOrDefaultAsync();

            ProductDetailVM productDetailVM = new ProductDetailVM
            {
                Product = product,
               
            };
            return View(productDetailVM);
        }

        //public async Task<IActionResult> Create(Comment comment)
        //{
        //    AppUser user = await _userManager.GetUserAsync(User);

        //    comment.AppUser = user;
        //    comment.AppUserId = user.Id;
        //    comment.CreateDate = DateTime.Now;

        //}
    }
}
