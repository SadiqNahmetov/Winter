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
    public class InstagramPhotoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public InstagramPhotoController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<InstagramPhoto> instagramPhotos = await _context.InstagramPhotos.Where(m => !m.IsDeleted).ToListAsync();
            return View(instagramPhotos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstagramPhoto instagramPhoto)
        {
            if (!ModelState.IsValid) return View();

            if (!instagramPhoto.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!instagramPhoto.Photo.CheckFileSize(200000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + instagramPhoto.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/instagramphoto", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await instagramPhoto.Photo.CopyToAsync(stream);
            }

            instagramPhoto.Image = fileName;

            await _context.InstagramPhotos.AddAsync(instagramPhoto);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            InstagramPhoto instagramPhoto = await _context.InstagramPhotos.FindAsync(id);

            if (instagramPhoto == null) return NotFound();

            return View(instagramPhoto);
        }





        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                InstagramPhoto instagram = await _context.InstagramPhotos.FirstOrDefaultAsync(m => m.Id == id);

                if (instagram is null) return NotFound();

                return View(instagram);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, InstagramPhoto instagramPhoto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(instagramPhoto);
                }
               InstagramPhoto instagramDb = await _context.InstagramPhotos.FindAsync(id);
                
                instagramDb.Image = instagramPhoto.Image;
                instagramDb.Social = instagramPhoto.Social;

                if (instagramPhoto.Photo != null)
                {
                    if (!instagramPhoto.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View();
                    }

                    if (!instagramPhoto.Photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View();
                    }
                    string fileName = Guid.NewGuid().ToString() + "_" + instagramPhoto.Photo.FileName;
                    InstagramPhoto dbInstagramPhoto = await _context.InstagramPhotos.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (dbInstagramPhoto is null) return NotFound();

                    if (dbInstagramPhoto.Photo == instagramPhoto.Photo)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/instagramphoto", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await instagramPhoto.Photo.CopyToAsync(stream);
                    }

                    instagramDb.Image = fileName;

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
            InstagramPhoto instagramPhoto = await GetByIdAsync(id);

            if (instagramPhoto == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", instagramPhoto.Image);


            Helper.DeleteFile(path);

            _context.InstagramPhotos.Remove(instagramPhoto);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<InstagramPhoto> GetByIdAsync(int id)
        {
            return await _context.InstagramPhotos.FindAsync(id);
        }

    }
}
