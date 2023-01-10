using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels.Blog
{
    public class BlogCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        public DateTime CreateDate { get; set; }
        public int BlogCategoryId { get; set; }
  
        public int TagId { get; set; }
        public List<Models.Tag> Tag { get; set; }
        public IEnumerable<int> Blog_TagList { get; set; }
    }
}
