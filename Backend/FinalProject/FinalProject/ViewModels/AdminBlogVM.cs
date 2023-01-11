using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels
{
    public class AdminBlogVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreateDate { get; set; }
        public int TagId { get; set; }
        public List<Tag> Tags { get; set; }
        public IEnumerable<BlogCategory> BlogCategories { get; set; }
    }
}
