using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStoreForWoman.DataAccess;
using OnlineStoreForWoman.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace OnlineStoreForWoman.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private ISession _session;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoryController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _session = httpContextAccessor.HttpContext.Session;

        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            string userId = _session.GetString("UserId");
            ApplicationUser userApp = new ApplicationUser();
            userApp.Id = userId;
            var user = await _userManager.FindByIdAsync(userApp.Id);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");

            }
            if (user.UserName != "admin")
            {
                return Json("You dont have a Access");
            }
            else
            {
                return View(await _context.Category.ToListAsync());
            }
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description,IsActive,Picture")] Category category)
        {
            if (category != null)
            {
                if (category.Picture != null)
                {
                    string folder = "CategoryImage/";
                    category.PicturePath = await UploadImage(folder, category.Picture);
                }


                category.CreatedOn = DateTime.Now;
                category.CreatedBy = User.Identity.Name;
                _context.Add(category);
                await _context.SaveChangesAsync();  
                return RedirectToAction(nameof(Index));

            }
            else
            {
                return View(category);

            }

        }


        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category category = new Category();
            category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                return View(category);
            }
            else
            {
                return View(category);
            }
        }


        // POST: Category/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (category.Id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.UpdatedOn = DateTime.Now;
                    category.UpdatedBy = User.Identity.Name;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
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
