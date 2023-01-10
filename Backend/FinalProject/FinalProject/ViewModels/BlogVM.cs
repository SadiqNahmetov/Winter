using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels
{
    public class BlogVM
    {
        public IEnumerable<Models.Blog> Blog { get; set; }
        public IEnumerable<Models.Blog> RecentPosts { get; set; }

        public IEnumerable<BlogCategory> BlogCategories { get; set; }
        public IEnumerable<Tag> Tags { get; set; }




    }
}
