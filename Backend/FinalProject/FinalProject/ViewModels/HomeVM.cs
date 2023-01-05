using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public IEnumerable<InstagramPhoto> InstagramPhotos { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Feature> Features { get; set; }
        public IEnumerable<Models.Product> Products { get; set; }





    }
}
