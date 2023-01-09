using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Service : BaseEntity
    {
        public string Image { get; set; }
        [Required(ErrorMessage = "Name can't be empty")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description can't be empty")]
        public string Description { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Image can't be empty")]
        public IFormFile Photo { get; set; }


    }
}
