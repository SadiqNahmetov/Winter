using FinalProject.Data;
using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.ViewModels.Product;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ProductController : Controller
    {
        #region Readonly
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        #endregion

        #region Constructor
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            List<Product> products = await _context.Products
                .Where(m => !m.IsDeleted)
                .Include(m => m.ProductImages)
                .Include(m => m.Category)
                .Include(m => m.Brand)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            ViewBag.take = take;

            List<ProductListVM> mapDatas = GetMapDatas(products);

            int count = await GetPageCount(take);

            Paginate<ProductListVM> result = new Paginate<ProductListVM>(mapDatas, page, count);

            return View(result);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await GetCategoriesAsync();
            ViewBag.brands = await GetBrandAsync();
            var data = await GetSizesAsync();

            ProductCreateVM productCreateVM = new ProductCreateVM()
            {
                Size = data
            };


            return View(productCreateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM product)
        {
            ViewBag.categories = await GetCategoriesAsync();
            ViewBag.brands = await GetBrandAsync();

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            foreach (var photo in product.Photos)
            {
                if (!photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image type");
                    return View(product);
                }


                if (!photo.CheckFileSize(500))
                {
                    ModelState.AddModelError("Photo", "Please choose correct image size");
                    return View(product);
                }

            }

            List<ProductImage> images = new List<ProductImage>();

            foreach (var photo in product.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/product", fileName);

                await Helper.SaveFile(path, photo);


                ProductImage image = new ProductImage
                {
                    Image = fileName,
                };

                images.Add(image);
            }

            images.FirstOrDefault().IsMain = true;

            decimal convertedPrice = decimal.Parse(product.Price.Replace(".", ","));

            Product newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = (int)convertedPrice,
                CreateDate = DateTime.Now,
                CategoryId = product.CategoryId,
                ProductImages = images,
                BrandId = product.BrandId,
            };

            await _context.Products.AddAsync(newProduct);

            await _context.SaveChangesAsync();

            // Product dbProduct = await _context.Products.FirstOrDefaultAsync(m => m.Id == 1);

            foreach (var item in product.Size.Where(m => m.IsSelected))
            {
                ProductSize productSize = new ProductSize
                {
                    ProductId = newProduct.Id,
                    SizeId = item.Id,
                };
                await _context.ProductSizes.AddAsync(productSize);
            }

            _context.ProductImages.UpdateRange(images);
            _context.Products.Update(newProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.ProductImages)
                .FirstOrDefaultAsync();

            if (product == null) return NotFound();

            foreach (var item in product.ProductImages)
            {
                string path = Helper.GetFilePath(_env.WebRootPath, "img", item.Image);
                Helper.DeleteFile(path);
                item.IsDeleted = true;
            }

            product.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        private decimal StringToDecimal(string str)
        {
            return decimal.Parse(str.Replace(".", ","));
        }
        #endregion

        #region Detail
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = await _context.Products
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.ProductImages)
                .Include(m => m.Category)
                .Include(m => m.Brand)
                .FirstOrDefaultAsync();

            List<ProductSize> product_Sizes = await _context.ProductSizes.Where(m => m.ProductId == id).ToListAsync();
            List<Size> sizes = new List<Size>();
            foreach (var size in product_Sizes)
            {
                Size dbSize = await _context.Sizes.Where(m => m.Id == size.SizeId).FirstOrDefaultAsync();
                sizes.Add(dbSize);
            }

            if (product == null)
            {
                return NotFound();
            }
            var data = await GetSizesAsync();

            ProductAdminDetailVM productDetail = new ProductAdminDetailVM
            {
                Id = product.Id,
                Title = product.Name,
                Price = product.Price,
                Description = product.Description,
                ProductImages = product.ProductImages,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
                Sizes = sizes,
            };




            return View(productDetail);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            ViewBag.brands = await GetBrandAsync();
            ViewBag.categories = await GetCategoriesAsync();

            Product dbProduct = await GetByIdAsync((int)id);

            return View(new ProductEditVM
            {
                Id = dbProduct.Id,
                Title = dbProduct.Name,
                Description = dbProduct.Description,
                Price = dbProduct.Price.ToString("0.#####").Replace(",", "."),
                CategoryId = dbProduct.CategoryId,
                Images = dbProduct.ProductImages,
                BrandId = dbProduct.BrandId,


            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditVM updatedProduct)
        {
            ViewBag.categories = await GetCategoriesAsync();
            ViewBag.brands = await GetBrandAsync();

            if (!ModelState.IsValid) return View(updatedProduct);

            Product dbProduct = await GetByIdAsync(id);

            if (updatedProduct.Photos != null)
            {

                foreach (var photo in updatedProduct.Photos)
                {
                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image type");
                        return View(updatedProduct);
                    }


                    if (!photo.CheckFileSize(500))
                    {
                        ModelState.AddModelError("Photo", "Please choose correct image size");
                        return View(updatedProduct);
                    }

                }

                foreach (var item in dbProduct.ProductImages)
                {
                    string path = Helper.GetFilePath(_env.WebRootPath, "img", item.Image);
                    Helper.DeleteFile(path);
                }


                List<ProductImage> images = new List<ProductImage>();

                foreach (var photo in updatedProduct.Photos)
                {

                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/product", fileName);

                    await Helper.SaveFile(path, photo);


                    ProductImage image = new ProductImage
                    {
                        Image = fileName,
                    };

                    images.Add(image);

                }

                images.FirstOrDefault().IsMain = true;

                dbProduct.ProductImages = images;

            }

            decimal convertedPrice = StringToDecimal(updatedProduct.Price);



            dbProduct.Name = updatedProduct.Title;
            dbProduct.Description = updatedProduct.Description;
            dbProduct.Price = (int)convertedPrice;
            dbProduct.CategoryId = updatedProduct.CategoryId;
            dbProduct.BrandId = updatedProduct.BrandId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Services
        private async Task<SelectList> GetCategoriesAsync()
        {
            IEnumerable<Category> categories = await _context.Categories.Where(m => !m.IsDeleted).ToListAsync();
            return new SelectList(categories, "Id", "Name");
        }
        private async Task<SelectList> GetBrandAsync()
        {
            IEnumerable<Brand> brands = await _context.Brands.Where(m => !m.IsDeleted).ToListAsync();
            return new SelectList(brands, "Id", "Name");
        }
        private async Task<List<Size>> GetSizesAsync()
        {
            List<Size> sizes = await _context.Sizes.Where(m => !m.IsDeleted).ToListAsync();
            return sizes;
        }

        private async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Where(m => !m.IsDeleted && m.Id == id)
                .Include(m => m.Category)
                .Include(m => m.ProductImages)
                .FirstOrDefaultAsync();
        }

        private List<ProductListVM> GetMapDatas(List<Product> products)
        {
            List<ProductListVM> productList = new List<ProductListVM>();

            foreach (var product in products)
            {
                ProductListVM newProduct = new ProductListVM
                {
                    Id = product.Id,
                    Title = product.Name,
                    Description = product.Description,
                    MainImage = product.ProductImages.Where(m => m.IsMain).FirstOrDefault()?.Image,
                    CategoryName = product.Category.Name,
                    Price = product.Price
                };

                productList.Add(newProduct);
            }

            return productList;
        }

        private async Task<int> GetPageCount(int take)
        {
            int productCount = await _context.Products.Where(m => !m.IsDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }
        #endregion
    }
}
