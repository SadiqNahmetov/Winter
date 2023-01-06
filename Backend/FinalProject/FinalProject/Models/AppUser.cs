using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }

        public Basket Basket { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
