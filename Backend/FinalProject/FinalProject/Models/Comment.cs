using System;

namespace FinalProject.Models
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
