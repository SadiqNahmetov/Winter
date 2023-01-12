using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ContactController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            Contact contact = await _context.Contacts.Where(m => !m.IsDeleted).FirstOrDefaultAsync();
            return View(contact);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Contact contact = await _context.Contacts.FindAsync(id);

            if (contact == null) return NotFound();

            return View(contact);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(int? id)
        {
            try
            {
                if (id == null) return BadRequest();

                Contact contactInfo = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);

                if (contactInfo == null) return NotFound();

                return View(contactInfo);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Contact contactInfo)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }

                Contact dbContactInfo = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbContactInfo == null) return NotFound();

                if (dbContactInfo.Phone.Trim().ToLower() == dbContactInfo.Phone.Trim().ToLower())
                {
                    return RedirectToAction(nameof(Index));
                }

                _context.Contacts.Update(contactInfo);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }

        }


        //[HttpGet]

        //public async Task<IActionResult> Update(int? id)
        //{
        //    try
        //    {
        //        if (id == null) return BadRequest();

        //        Contact contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);

        //        if (contact == null) return NotFound();

        //        return View(contact);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Message = ex.Message;
        //        return View();
        //    }

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(int id, Contact contact)
        //{
        //    try
        //    {

        //        if (!ModelState.IsValid)
        //        {
        //            return View();
        //        }

        //        Contact dbContact = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

        //        if (dbContact == null) return NotFound();

        //        if (dbContact.Address.Trim().ToLower() == contact.Address.Trim().ToLower())
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }


        //        _context.Contacts.Update(contact);

        //        await _context.SaveChangesAsync();

        //        return RedirectToAction(nameof(Index));

        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Message = ex.Message;
        //        return View();
        //    }

        //}


    }
}
