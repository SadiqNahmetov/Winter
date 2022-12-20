using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public abstract class BaseEntity 
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

    }
}
