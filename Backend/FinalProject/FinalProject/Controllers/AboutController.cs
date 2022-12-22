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
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;

        public AboutController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            AboutMission aboutMission = await _context.AboutMissions.Where(m => !m.IsDeleted).FirstOrDefaultAsync();
            AboutVision aboutVision = await _context.AboutVisions.Where(m => !m.IsDeleted).FirstOrDefaultAsync();


            AboutVM aboutVM = new AboutVM()
            {
                AbourMission = aboutMission,
                AboutVision = aboutVision
               

            };
            return View(aboutVM);
        }
    }
}
