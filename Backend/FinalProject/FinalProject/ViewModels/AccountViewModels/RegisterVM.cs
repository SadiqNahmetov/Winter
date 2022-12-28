using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewModels.AccountViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Fullname can't be empty")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Name can't be empty")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string RepeatPassword { get; set; }


    }
}
