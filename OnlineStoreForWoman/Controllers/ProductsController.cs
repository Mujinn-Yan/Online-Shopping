using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStoreForWoman.DataAccess;
using OnlineStoreForWoman.Models;

namespace OnlineStoreForWoman.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ApplicationDbContext _context;

        public ProductsController(ILogger<ProductsController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductView(int proId)
        {
            try
            {
                ViewBag.products = _context.Product.Where(x => x.Id == proId).Include(x => x.Category).FirstOrDefault();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }
            
        }
        public IActionResult Products(int catId)
        {
            try
            {
                ViewBag.products = _context.Product.Include(x => x.Category).Where(x => x.CategoryID == catId).ToList();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }
            
        }

        //ADD RECENT VIEWS PRODUCT IN DB
        public void AddRecentViewProduct(int pid)
        {
            if (TempShpData.UserID > 0)
            {
                RecentlyView Rv = new RecentlyView();
                //Rv.CustomerID = TempShpData.UserID;
                Rv.ProductID = pid;
                Rv.ViewDate = DateTime.Now;
                _context.RecentlyView.Add(Rv);
                _context.SaveChanges();
            }
        }

        public IActionResult getProductByCategoryId(int catId)
        {
            try
            {
                List<Product> products = _context.Product.Include(x => x.Category).Where(x => x.CategoryID == catId).ToList();
                return Json(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }           
        }
    }
}
