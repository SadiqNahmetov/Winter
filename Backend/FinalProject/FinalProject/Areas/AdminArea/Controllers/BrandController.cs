using FinalProject.Data;
using FinalProject.Helpers;
using FinalProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async  Task<IActionResult> Index()
        {
            IEnumerable<Brand> brands = await _context.Brands.Where(m=> !m.IsDeleted).ToListAsync();
            return View(brands);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid) return View();

            if (!brand.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!brand.Photo.CheckFileSize(200000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + brand.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/brand", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await brand.Photo.CopyToAsync(stream);
            }

            brand.Image = fileName;

            await _context.Brands.AddAsync(brand);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FindAsync(id);

            if (brand == null) return NotFound();

            return View(brand);
        }



        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Brand brand = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);

                if (brand is null) return NotFound();

                return View(brand);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Brand brand)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(brand);
                }
                Brand brandDb = await _context.Brands.FindAsync(id);
                brandDb.Image = brand.Image;
                if (brand.Photo != null)
                {
                    if (!brand.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View();
                    }

                    if (!brand.Photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View();
                    }
                    string fileName = Guid.NewGuid().ToString() + "_" + brand.Photo.FileName;
                    Brand dbBrand = await _context.Brands.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (dbBrand is null) return NotFound();

                    if (dbBrand.Photo == brand.Photo)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/brand", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await brand.Photo.CopyToAsync(stream);
                    }

                    brandDb.Image = fileName;

                }

                await _context.SaveChangesAsync();
                string pathh = Helper.GetFilePath(_env.WebRootPath, "assets/images/brand", brandDb.Image);

                Helper.DeleteFile(pathh);

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
            Brand brand = await _context.Brands
                .Where(m => !m.IsDeleted && m.Id == id)
                .FirstOrDefaultAsync();

            if (brand == null) return NotFound();

           
                string path = Helper.GetFilePath(_env.WebRootPath, "img", brand.Image);
                Helper.DeleteFile(path);
                brand.IsDeleted = true;
          
            string pathh = Helper.GetFilePath(_env.WebRootPath, "assets/images/brand", brand.Image);

            Helper.DeleteFile(pathh);


            brand.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }




        private async Task<Brand> GetByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }
    }
}
