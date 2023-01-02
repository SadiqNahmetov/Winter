using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<InstagramPhoto> InstagramPhotos { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<AboutMission> AboutMissions { get; set; }
        public DbSet<AboutVision> AboutVisions { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<SendMessage> SendMessages { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }

        public DbSet<BlogCategory> BlogCategories { get; set; }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketProduct> BasketProducts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


















    }
}
