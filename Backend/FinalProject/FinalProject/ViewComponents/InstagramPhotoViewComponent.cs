using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.ViewComponents
{
    public class InstagramPhotoViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly LayoutService _layoutService;

        public InstagramPhotoViewComponent(LayoutService layoutService ,AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<InstagramPhoto> instagramPhotos = await _context.InstagramPhotos.Where(m => !m.IsDeleted).ToListAsync();

            InstagramPhotoVM instagramPhotoVM = new InstagramPhotoVM
            {
               InstagramPhotos = instagramPhotos
            };

            return await Task.FromResult(View(instagramPhotoVM));
        }
    }
}
