using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Brand : BaseEntity
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
