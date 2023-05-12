using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStoreForWoman.DataAccess;
using OnlineStoreForWoman.Entities;
using OnlineStoreForWoman.Models;
using Category = OnlineStoreForWoman.Models.Category;
using Product = OnlineStoreForWoman.Models.Product;

namespace OnlineStoreForWoman.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ISession _session;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _session = httpContextAccessor.HttpContext.Session;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {

            var products = await _context.Product.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                //.Include(p => p.SubCategory)
                //.Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {

            List<Category> categories = _context.Category.ToList();
            List<SelectListItem> categoryListItems = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            ViewBag.Category = categoryListItems;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {

            try
            {
                if (model != null)
                {
                    if (model.Picture != null)
                    {
                        string folder = "CategoryImage/";
                        model.PicturePath = await UploadImage(folder, model.Picture);
                    }

                    var product = new Product
                    {
                        Name = model.Name,
                        Category = null,
                        CategoryID = model.CategoryID,
                        UnitPrice = model.UnitPrice,
                        OldPrice = model.OldPrice,
                        Discount = model.Discount,
                        UnitInStock = model.UnitInStock,
                        ShortDescription = model.ShortDescription,
                        PicturePath = model.PicturePath,
                        Note = model.Note,
                        Picture = model.Picture // You may need to handle the file upload separately
                    };

                    _context.Product.Add(product);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true });
                }
                else
                {
                    return Json("Something wrong....");

                }
                // Your logic to save the product
                // For example:

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        //public async Task<IActionResult> Create(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (product != null)
        //        {
        //            _context.Add(product);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(product);
        //}
        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (product.Id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productModel = await _context.Product.FindAsync(product.Id);
                    if (productModel == null)
                    {
                        productModel.Name = product.Name;
                        productModel.OldPrice = product.OldPrice;
                        productModel.CategoryID = product.CategoryID;
                        productModel.PicturePath = productModel.PicturePath;
                        productModel.ShortDescription = product.ShortDescription;
                        productModel.Note = product.Note;
                        productModel.UnitInStock = product.UnitInStock;
                        productModel.Discount = product.Discount;


                        _context.Update(productModel);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }// POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "" + folderPath;
        }
    }
}
