using FinalProject.Data;
using FinalProject.Helpers;
using FinalProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BlogCategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogCategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<BlogCategory> blogCategory = await _context.BlogCategories.Where(m => !m.IsDeleted).ToListAsync();
            return View(blogCategory);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCategory blogCategory)
        {
            if (!ModelState.IsValid) return View();


            await _context.BlogCategories.AddAsync(blogCategory);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            BlogCategory blogCategory = await _context.BlogCategories.FindAsync(id);

            if (blogCategory == null) return NotFound();

            return View(blogCategory);
        }



        [HttpGet]
       
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                BlogCategory blogCategory = await _context.BlogCategories.FirstOrDefaultAsync(m => m.Id == id);

                if (blogCategory == null) return NotFound();

                return View(blogCategory);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BlogCategory blogCategory)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }

                BlogCategory dbBlogCategory = await _context.BlogCategories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbBlogCategory == null) return NotFound();

                if (dbBlogCategory.Name.Trim().ToLower() == blogCategory.Name.Trim().ToLower())
                {
                    return RedirectToAction(nameof(Index));
                }

                _context.BlogCategories.Update(blogCategory);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            BlogCategory blogCategory = await _context.BlogCategories
                .Where(m => !m.IsDeleted && m.Id == id)
                .FirstOrDefaultAsync();

            if (blogCategory == null) return NotFound();



            blogCategory.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }




        private async Task<BlogCategory> GetByIdAsync(int id)
        {
            return await _context.BlogCategories.FindAsync(id);
        }
    }
}
