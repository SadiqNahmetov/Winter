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
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Service> services = await _context.Services.Where(m => !m.IsDeleted).ToListAsync();

            return View(services);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid) return View();

            if (!service.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Please choose correct image type");
                return View();
            }

            if (!service.Photo.CheckFileSize(200000))
            {
                ModelState.AddModelError("Photo", "Please choose correct image size");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "_" + service.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/service", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await service.Photo.CopyToAsync(stream);
            }

            service.Image = fileName;

            await _context.Services.AddAsync(service);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }






        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Service service = await _context.Services.FindAsync(id);

            if (service == null) return NotFound();

            return View(service);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Service service = await _context.Services.FirstOrDefaultAsync(m => m.Id == id);

                if (service is null) return NotFound();

                return View(service);

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Service service)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(service);
                }
                Service serviceDb = await _context.Services.FindAsync(id);
                serviceDb.Title = service.Title;
                service.Description = service.Description;

                if (service.Photo != null)
                {
                    if (!service.Photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View();
                    }

                    if (!service.Photo.CheckFileSize(20000))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View();
                    }
                    string fileName = Guid.NewGuid().ToString() + "_" + service.Photo.FileName;
                    Service dbService = await _context.Services.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (dbService is null) return NotFound();

                    if (dbService.Photo == service.Photo)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/service", fileName);
                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await service.Photo.CopyToAsync(stream);
                    }

                    serviceDb.Image = fileName;

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
            Service service = await GetByIdAsync(id);

            if (service == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "img", service.Image);


            Helper.DeleteFile(path);

            _context.Services.Remove(service);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }





        private async Task<Service> GetByIdAsync(int id)
        {
            return await _context.Services.FindAsync(id);
        }
    }
}
