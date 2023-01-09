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
    public class BlogDetailController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public BlogDetailController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int? id)
        {
            Blog blog = await _context.Blogs
               .Where(m => !m.IsDeleted && m.Id == id)
               .Include(m=> m.BlogCategory)
               .Include(m=> m.BlogTags)
               .ThenInclude(m=> m.Tag)
               .Include(m=> m.BlogComments)
               .ThenInclude(m=> m.AppUser)
               .FirstOrDefaultAsync();

            IEnumerable<Blog> recentPosts = await _context.Blogs
                .Where(m => !m.IsDeleted)
                .OrderByDescending(m => m.Id).ToListAsync();

            IEnumerable<BlogCategory> blogCategories = await _context.BlogCategories.Where(m => !m.IsDeleted).ToListAsync();

            List<BlogTag> blogTags = await _context.BlogTags.Where(m => !m.IsDeleted && m.BlogId == id).ToListAsync();

            IEnumerable<BlogComment> blogComments = await _context.BlogComments.Where(m => !m.IsDeleted && m.BlogId == id).ToListAsync();

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
                Tags = tags,
                BlogComment = new BlogComment(),
                BlogComments = blogComments,
                   
            };
            return View(blogDetailVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(BlogComment blogComment)
        {
            Blog blog = await _context.Blogs
              .Where(m => !m.IsDeleted && m.Id == blogComment.BlogId)
               .Include(m => m.BlogCategory)
               .Include(m => m.BlogTags)
               .ThenInclude(m => m.Tag)
               .Include(m => m.BlogComments)
               .ThenInclude(m => m.AppUser)
               .FirstOrDefaultAsync();

            AppUser user = await _userManager.GetUserAsync(User);
            Blog blog1 = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == blogComment.BlogId);


            blogComment.AppUser = user;
            blogComment.AppUserId = user.Id;
            blogComment.CreateDate = DateTime.Now;
            blogComment.Blog = blog;

            await _context.BlogComments.AddAsync(blogComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = blog.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id)
        {

            BlogComment blogComment = await _context.BlogComments.FirstOrDefaultAsync(n => n.Id == id);


            blogComment.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { id = blogComment.BlogId });
        }

    }
}
