﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.AdminArea.Controllers
{
    public class SliderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
