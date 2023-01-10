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
    public class AboutVisionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public AboutVisionController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<AboutVision> aboutVisions = await _context.AboutVisions.Where(m => !m.IsDeleted).ToListAsync();
            return View(aboutVisions);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutVision aboutVision)
        {
            if (!ModelState.IsValid) return View();

            if (!aboutVision.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!aboutVision.Photo.CheckFileSize(200000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + aboutVision.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/about", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await aboutVision.Photo.CopyToAsync(stream);
            }

            aboutVision.Image = fileName;

            await _context.AboutVisions.AddAsync(aboutVision);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            AboutVision aboutVision = await _context.AboutVisions.FindAsync(id);

            if (aboutVision == null) return NotFound();

            return View(aboutVision);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                AboutVision aboutVision = await _context.AboutVisions.FirstOrDefaultAsync(m => m.Id == id);

                if (aboutVision is null) return NotFound();

                return View(aboutVision);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, AboutVision aboutVision)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(aboutVision);
                }
                AboutVision aboutVisionDb = await _context.AboutVisions.FindAsync(id);
                aboutVisionDb.Image = aboutVision.Image;
                aboutVisionDb.Title = aboutVision.Title;
                aboutVisionDb.Description = aboutVision.Description;

                if (aboutVision.Photo != null)
                {
                    if (!aboutVision.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View();
                    }

                    if (!aboutVision.Photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View();
                    }
                    string fileName = Guid.NewGuid().ToString() + "_" + aboutVision.Photo.FileName;
                    AboutVision dbAboutVision = await _context.AboutVisions.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (dbAboutVision is null) return NotFound();

                    if (dbAboutVision.Photo == aboutVision.Photo)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/about", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await aboutVision.Photo.CopyToAsync(stream);
                    }

                    aboutVisionDb.Image = fileName;

                }

                await _context.SaveChangesAsync();
                string pathh = Helper.GetFilePath(_env.WebRootPath, "assets/images/about", aboutVisionDb.Image);

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
            AboutVision aboutVision = await _context.AboutVisions
                .Where(m => !m.IsDeleted && m.Id == id)
                .FirstOrDefaultAsync();

            if (aboutVision == null) return NotFound();


            string path = Helper.GetFilePath(_env.WebRootPath, "img", aboutVision.Image);
            Helper.DeleteFile(path);
            aboutVision.IsDeleted = true;

            string pathh = Helper.GetFilePath(_env.WebRootPath, "assets/images/about", aboutVision.Image);

            Helper.DeleteFile(pathh);


            aboutVision.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }




        private async Task<AboutVision> GetByIdAsync(int id)
        {
            return await _context.AboutVisions.FindAsync(id);
        }


    }
}
