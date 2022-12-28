using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<BlogTag> BlogTags { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

    }
}
