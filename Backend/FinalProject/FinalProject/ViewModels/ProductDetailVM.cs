using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels
{
    public class ProductDetailVM
    {
        public Models.Product Product { get; set; }
        public Comment Comment { get; set; }
    }
}
