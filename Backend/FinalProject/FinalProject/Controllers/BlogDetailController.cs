using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class BlogDetailController : Controller
    {
        private readonly AppDbContext _context;

        public BlogDetailController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            Blog blog = await _context.Blogs
               .Where(m => !m.IsDeleted && m.Id == id)
               .FirstOrDefaultAsync();

            IEnumerable<Blog> recentPosts = await _context.Blogs
                .Where(m => !m.IsDeleted)
                .OrderByDescending(m => m.Id).ToListAsync();

            IEnumerable<BlogCategory> blogCategories = await _context.BlogCategories.Where(m => !m.IsDeleted).ToListAsync();


            BlogDetailVM blogDetailVM = new BlogDetailVM
                  {
                      Blog = blog,
                      RecentPosts = recentPosts,
                      BlogCategories = blogCategories,
                    
                  };
            return View(blogDetailVM);
        }
    }
}
