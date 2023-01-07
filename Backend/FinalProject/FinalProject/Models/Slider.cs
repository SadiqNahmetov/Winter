using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Slider : BaseEntity
    {
        public string Image { get; set; }
        [Required(ErrorMessage = "Name can't be empty")]
        public string Title { get; set; }
        public string Descirption { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Image can't be empty")]
        public IFormFile Photo { get; set; }

    }
}
