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
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> blog = await _context.Blogs.Where(m => !m.IsDeleted).ToListAsync();
            IEnumerable<BlogCategory> blogCategories = await _context.BlogCategories.Where(m => !m.IsDeleted).ToListAsync();

            IEnumerable<Blog> recentPosts = await _context.Blogs.Where(m => !m.IsDeleted).OrderByDescending(m => m.Id).ToListAsync();
            List<Tag> tags = await _context.Tags.Where(m => !m.IsDeleted).ToListAsync();

            BlogVM blogVM = new BlogVM
            {
                Blog = blog,
                BlogCategories = blogCategories,
                RecentPosts = recentPosts,
                Tags = tags
            };
            return View(blogVM);
        }

      
    }
}
