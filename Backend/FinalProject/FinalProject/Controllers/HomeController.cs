using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{

    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
           
            IEnumerable<Slider> sliders = await _context.Sliders.Where(m => !m.IsDeleted).ToListAsync();


            HomeVM model = new HomeVM
            {
                Sliders = sliders,
            };


            return View(model);
        }

       

       
    }
}
