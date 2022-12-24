using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            Map map = await _context.Maps.Where(m => !m.IsDeleted).FirstOrDefaultAsync();

            Contact contact = await _context.Contacts.Where(m => !m.IsDeleted).FirstOrDefaultAsync();
            SendMessage sendMessage = await _context.SendMessages.Where(m => !m.IsDeleted).FirstOrDefaultAsync();


            ContactVM contactVM = new ContactVM {
               
             Map = map,
             Contact = contact,
             
             
            };

            return View(contactVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SendMessage sendMessage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction(nameof(Index));
                }

                bool isExist = await _context.SendMessages.AnyAsync(m => m.Name.Trim() == sendMessage.Name.Trim()
                && m.Surname.Trim() == sendMessage.Surname.Trim()
                && m.Email.Trim() == sendMessage.Email.Trim()
                && m.Subject.Trim() == sendMessage.Subject.Trim()
                && m.Message.Trim() == sendMessage.Message.Trim());

                if (isExist)
                {
                    ModelState.AddModelError("Name", "Subject already exist");
                    return View();
                }

                await _context.SendMessages.AddAsync(sendMessage);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View();
            }


        }
    }
}
