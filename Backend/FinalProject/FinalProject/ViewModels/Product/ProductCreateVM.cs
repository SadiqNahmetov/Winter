using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels.Product
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int BrandId { get; set; }
        public string DiscountPrice { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public List<IFormFile> Photos { get; set; }
        public int SizeId { get; set; }
        public List<Size> Size { get; set; }
        public IEnumerable<int> ProductSizeList { get; set; }


    }
}
