using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class DashboardController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }





    }  
       
}
