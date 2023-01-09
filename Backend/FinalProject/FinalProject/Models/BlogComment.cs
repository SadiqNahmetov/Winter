using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class BlogComment : BaseEntity
    {
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }
}
