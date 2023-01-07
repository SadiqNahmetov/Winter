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
                .ThenInclude(m=> m.Size)
                .Include(m => m.Comments)
                .ThenInclude(m => m.AppUser)
                .FirstOrDefaultAsync();

            IEnumerable<Comment> comments = await _context.Comments.Where(m => !m.IsDeleted && m.ProductId == id).ToListAsync();


            ProductDetailVM productDetailVM = new ProductDetailVM
            {
                Product = product,
                Comment = new Comment(),
                DbComments = comments,
            };
            return View(productDetailVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            Product product = await _context.Products
               .Where(m => !m.IsDeleted && m.Id == comment.ProductId)
               .Include(m => m.ProductImages)
               .Include(m => m.Category)
               .Include(m => m.Brand)
               .Include(m => m.ProductSizes)
               .ThenInclude(m => m.Size)
               .Include(m => m.Comments)
               .ThenInclude(m => m.AppUser)
               .FirstOrDefaultAsync();

            AppUser user = await _userManager.GetUserAsync(User);
            Product product1 = await _context.Products
                 .Include(m => m.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == comment.ProductId);


            comment.AppUser = user;
            comment.AppUserId = user.Id;
            comment.CreateDate = DateTime.Now;
            comment.Product = product;

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = product.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id)
        {

            Comment comment = await _context.Comments.FirstOrDefaultAsync(n => n.Id == id);


            comment.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = comment.ProductId});
        }

        

    }
}
