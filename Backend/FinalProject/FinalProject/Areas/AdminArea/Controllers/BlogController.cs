using FinalProject.Data;
using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Blog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Blog> blogs = await _context.Blogs
                .Where(m => !m.IsDeleted)
                .Include(m=> m.BlogCategory)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            ViewBag.take = take;

            List<BlogListVM> mapDatas = GetMapDatas(blogs);

            int count = await GetPageCount(take);

            Paginate<BlogListVM> result = new Paginate<BlogListVM>(mapDatas, page, count);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await GetCategoriesAsync();
            var data = await GetTagAsync();

            BlogCreateVM blogCreateVM = new BlogCreateVM()
            {
                Tag = data
            };


            return View(blogCreateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM blog)
        {
            ViewBag.categories = await GetCategoriesAsync();


            if (!ModelState.IsValid) return View(blog);

            if (!blog.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View(blog);
            }

            if (!blog.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View(blog);
            }

            string fileName = Guid.NewGuid().ToString() + "_" + blog.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/blog", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await blog.Photo.CopyToAsync(stream);
            }

            Blog newBlog = new Blog
            {
                Title = blog.Title,
                Description = blog.Description,
                BlogCategoryId = blog.BlogCategoryId,
                Image = fileName,
                CreateDate = DateTime.Now
            };

            await _context.Blogs.AddAsync(newBlog);

            await _context.SaveChangesAsync();

            foreach (var item in blog.Tag.Where(m => m.IsSelected))
            {
                BlogTag blogTag = new BlogTag
                {
                    BlogId = newBlog.Id,
                    TagId = item.Id,
                };
                await _context.BlogTags.AddAsync(blogTag);
            }

            _context.Blogs.Update(newBlog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();


            Blog blog = await _context.Blogs
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.BlogTags)
                .Include(m => m.BlogCategory)
                .FirstOrDefaultAsync();

            List<BlogTag> blogTags = await _context.BlogTags.Where(m => m.BlogId == id).ToListAsync();
            List<Tag> tags = new List<Tag>();
            foreach (var tag in blogTags)
            {
                Tag dbTag = await _context.Tags.Where(m => m.Id == tag.TagId).FirstOrDefaultAsync();
                tags.Add(dbTag);
            }

            if (blog == null)
            {
                return NotFound();
            }
            var data = await GetTagAsync();

            AdminBlogVM adminBlogVM = new AdminBlogVM
            {
                Id = blog.Id,
                Image = blog.Image,
                Title = blog.Title,
                Description = blog.Description,
                CreateDate = DateTime.Now,
                CategoryName = blog.BlogCategory.Name,
                Tags = tags,
            };

            return View(adminBlogVM);
        }

        [HttpGet]
        
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                ViewBag.blogCategories = await GetCategoriesAsync();

                Blog blog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);

                if (blog is null) return NotFound();

                return View(new BlogEditVM
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Description = blog.Description,
                    BlogCategoryId = blog.BlogCategoryId,
                    Image = blog.Image
                });

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BlogEditVM updateBlog)
        {
            try
            {
                ViewBag.blogCategories = await GetCategoriesAsync();

                if (!ModelState.IsValid)
                {
                    return View(updateBlog);
                }
                Blog blog = await GetByIdAsync(id);
                if (updateBlog.Photo != null)
                {
                    if (!updateBlog.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(updateBlog);
                    }

                    if (!updateBlog.Photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View(updateBlog);
                    }
                    string fileName = Guid.NewGuid().ToString() + "_" + updateBlog.Photo.FileName;
                    Blog blogDb = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (blogDb is null) return NotFound();

                    if (blogDb.Image == updateBlog.Image)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/blog", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await updateBlog.Photo.CopyToAsync(stream);
                    }

                    blog.Image = fileName;

                }

                blog.Title = updateBlog.Title;
                blog.Description = updateBlog.Description;
                blog.BlogCategoryId = updateBlog.BlogCategoryId;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }



        private async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }

        private async Task<SelectList> GetCategoriesAsync()
        {
            IEnumerable<BlogCategory> categories = await _context.BlogCategories.Where(m => !m.IsDeleted).ToListAsync();
            
            return new SelectList(categories, "Id", "Name");
        }

        private async Task<List<Tag>> GetTagAsync()
        {
            List<Tag> tags = await _context.Tags.Where(m => !m.IsDeleted).ToListAsync();
            return tags;
        }
        private List<BlogListVM> GetMapDatas(List<Blog> blogs)
        {
            List<BlogListVM> blogLists = new List<BlogListVM>();

            foreach (var blog in blogs)
            {
                BlogListVM newBlog = new BlogListVM
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Image = blog.Image,
                    CreateDate = blog.CreateDate,
                    Description = blog.Description,
                    Category = blog.BlogCategory.Name
                    
                };

                blogLists.Add(newBlog);
            }

            return blogLists;
        }

        private async Task<int> GetPageCount(int take)
        {
            int blogCount = await _context.Blogs.Where(m => !m.IsDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)blogCount / take);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Blog blog = await GetByIdAsync(id);

            if (blog == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", blog.Image);


            Helper.DeleteFile(path);

            _context.Blogs.Remove(blog);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



     
    }
}
