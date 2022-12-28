using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class BlogCategory : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Blog> Blogs { get; set; }



    }
}
