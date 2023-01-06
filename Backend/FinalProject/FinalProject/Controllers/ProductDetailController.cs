using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
                .Include(m => m.Comments)
                .ThenInclude(m => m.AppUser)
                .FirstOrDefaultAsync();

            ProductDetailVM productDetailVM = new ProductDetailVM
            {
                Product = product,
                Comment = new Comment()
            };
            return View(productDetailVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Comment comment)
        {


            AppUser user = await _userManager.GetUserAsync(User);
            Product product = await _context.Products.FirstOrDefaultAsync(m => m.Id == comment.ProductId);


            comment.AppUser = user;
            comment.AppUserId = user.Id;
            comment.CreateDate = DateTime.Now;
            comment.Product = product;

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteComment(int id)
        {
           
            Comment comment = await _context.Comments.FirstOrDefaultAsync(n => n.Id == id);

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
