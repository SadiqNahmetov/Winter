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
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AboutController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<AboutMission> aboutMissions = await _context.AboutMissions.Where(m => !m.IsDeleted).ToListAsync();
            return View(aboutMissions);
         
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutMission aboutMission)
        {
            if (!ModelState.IsValid) return View();

            if (!aboutMission.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!aboutMission.Photo.CheckFileSize(200000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + aboutMission.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/about", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await aboutMission.Photo.CopyToAsync(stream);
            }

            aboutMission.Image = fileName;

            await _context.AboutMissions.AddAsync(aboutMission);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            AboutMission aboutMission = await _context.AboutMissions.FindAsync(id);

            if (aboutMission == null) return NotFound();

            return View(aboutMission);
        }



        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                AboutMission aboutMission = await _context.AboutMissions.FirstOrDefaultAsync(m => m.Id == id);

                if (aboutMission is null) return NotFound();

                return View(aboutMission);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, AboutMission aboutMission)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(aboutMission);
                }
                AboutMission aboutMissionDb = await _context.AboutMissions.FindAsync(id);
                aboutMissionDb.Image = aboutMission.Image;
                aboutMissionDb.Title = aboutMission.Title;
                aboutMissionDb.Description = aboutMission.Description;

                if (aboutMission.Photo != null)
                {
                    if (!aboutMission.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View();
                    }

                    if (!aboutMission.Photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View();
                    }
                    string fileName = Guid.NewGuid().ToString() + "_" + aboutMission.Photo.FileName;
                    AboutMission dbAboutMission = await _context.AboutMissions.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (dbAboutMission is null) return NotFound();

                    if (dbAboutMission.Photo == aboutMission.Photo)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/about", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await aboutMission.Photo.CopyToAsync(stream);
                    }

                    aboutMissionDb.Image = fileName;

                }

                await _context.SaveChangesAsync();
                string pathh = Helper.GetFilePath(_env.WebRootPath, "assets/images/about", aboutMissionDb.Image);

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
            AboutMission aboutMission = await _context.AboutMissions
                .Where(m => !m.IsDeleted && m.Id == id)
                .FirstOrDefaultAsync();

            if (aboutMission == null) return NotFound();


            string path = Helper.GetFilePath(_env.WebRootPath, "img", aboutMission.Image);
            Helper.DeleteFile(path);
            aboutMission.IsDeleted = true;

            string pathh = Helper.GetFilePath(_env.WebRootPath, "assets/images/about", aboutMission.Image);

            Helper.DeleteFile(pathh);


            aboutMission.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }




        private async Task<AboutMission> GetByIdAsync(int id)
        {
            return await _context.AboutMissions.FindAsync(id);
        }
    }
}
