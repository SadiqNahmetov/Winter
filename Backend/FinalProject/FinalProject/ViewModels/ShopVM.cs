using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels
{
    public class ShopVM
    {
        public List<Models.Product> Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Size> Sizes { get; set; }
        public IEnumerable<InstagramPhoto> InstagramPhotos { get; set; }
    }
}
