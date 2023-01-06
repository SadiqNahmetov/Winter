using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels.Basket
{
    public class BasketProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public Brand  Brand { get; set; }
    }
}
