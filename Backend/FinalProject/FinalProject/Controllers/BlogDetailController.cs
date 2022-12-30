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

            List<BlogTag> blogTags = await _context.BlogTags.Where(m => !m.IsDeleted && m.BlogId == id).ToListAsync();

            List<Tag> tags = new List<Tag>();
            foreach (var tag in blogTags)
            {
                Tag dbTag = await _context.Tags.Where(m => m.Id == tag.TagId).FirstOrDefaultAsync();
                tags.Add(dbTag);
            }

            BlogDetailVM blogDetailVM = new BlogDetailVM
                  {
                      Blog = blog,
                      RecentPosts = recentPosts,
                      BlogCategories = blogCategories,
                      Tags = tags
                    
                  };
            return View(blogDetailVM);
        }
    }
}
