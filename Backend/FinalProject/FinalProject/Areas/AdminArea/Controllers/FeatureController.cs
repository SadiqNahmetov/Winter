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
    public class FeatureController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FeatureController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Feature> feature = await _context.Features.Where(m => !m.IsDeleted).ToListAsync();
            return View(feature);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feature feature)
        {
            if (!ModelState.IsValid) return View();

            if (!feature.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!feature.Photo.CheckFileSize(200000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + feature.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/product", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await feature.Photo.CopyToAsync(stream);
            }

            feature.Image = fileName;

            await _context.Features.AddAsync(feature);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Feature feature = await _context.Features.FindAsync(id);

            if (feature == null) return NotFound();

            return View(feature);
        }



        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Feature feature = await _context.Features.FirstOrDefaultAsync(m => m.Id == id);

                if (feature is null) return NotFound();

                return View(feature);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Feature feature)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(feature);
                }
                Feature featureDb = await _context.Features.FindAsync(id);
              
                if (feature.Photo != null)
                {
                    if (!feature.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View();
                    }

                    if (!feature.Photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View();
                    }
                    string fileName = Guid.NewGuid().ToString() + "_" + feature.Photo.FileName;
                    Feature dbFeature = await _context.Features.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (dbFeature is null) return NotFound();

                    if (dbFeature.Photo == feature.Photo)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/product", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await feature.Photo.CopyToAsync(stream);
                    }

                    featureDb.Image = fileName;

                }

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
            Feature feature = await _context.Features
                .Where(m => !m.IsDeleted && m.Id == id)
                .FirstOrDefaultAsync();

            if (feature == null) return NotFound();

            
            
                string path = Helper.GetFilePath(_env.WebRootPath, "img", feature.Image);
                Helper.DeleteFile(path);
                feature.IsDeleted = true;
            

            feature.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


    }
}
