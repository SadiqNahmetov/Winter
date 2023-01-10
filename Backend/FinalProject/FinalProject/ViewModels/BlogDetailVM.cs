using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels
{
    public class BlogDetailVM
    {
        public Models.Blog Blog { get; set; }
        public IEnumerable<Models.Blog> RecentPosts { get; set; }

        public IEnumerable<BlogCategory> BlogCategories { get; set; }
        public List<Tag> Tags { get; set; }

        public BlogComment BlogComment { get; set; }
        public IEnumerable<BlogComment> BlogComments { get; set; }



    }
}
