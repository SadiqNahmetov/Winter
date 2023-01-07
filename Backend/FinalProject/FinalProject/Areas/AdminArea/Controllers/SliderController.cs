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
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _context.Sliders.Where(m => !m.IsDeleted).ToListAsync();
            return View(sliders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            if (!slider.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!slider.Photo.CheckFileSize(200000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/slider", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await slider.Photo.CopyToAsync(stream);
            }

            slider.Image = fileName;

            await _context.Sliders.AddAsync(slider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Slider slider = await _context.Sliders.FindAsync(id);

            if (slider == null) return NotFound();

            return View(slider);
        }

        
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

                if (slider is null) return NotFound();

                return View(slider);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Slider slider)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(slider);
                }
                Slider sliderDb = await _context.Sliders.FindAsync(id);
                sliderDb.Title = slider.Title;
                sliderDb.Descirption = slider.Descirption;
               
                if (slider.Photo != null)
                {
                    if (!slider.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View();
                    }

                    if (!slider.Photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View();
                    }
                    string fileName = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;
                    Slider dbSlider = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (dbSlider is null) return NotFound();

                    if (dbSlider.Photo == slider.Photo)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/slider", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await slider.Photo.CopyToAsync(stream);
                    }

                    sliderDb.Image = fileName;

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






        public async Task<IActionResult> Delete(int id)
        {
            Slider slider = await GetByIdAsync(id);

            if (slider == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", slider.Image);


            Helper.DeleteFile(path);

            _context.Sliders.Remove(slider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        private async Task<Slider> GetByIdAsync(int id)
        {
            return await _context.Sliders.FindAsync(id);
        }
    }
}
