using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStoreForWoman.DataAccess;
using OnlineStoreForWoman.DTOs;
using OnlineStoreForWoman.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineStoreForWoman.Controllers
{
    //[Authorize("User")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        List<Cart> cartlist = new List<Cart>();
        private ISession _session;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {

            _context = context;
            _logger = logger;
            _userManager = userManager;
            _session = httpContextAccessor.HttpContext.Session;

        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = _session.GetString("UserId");
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    ViewBag.User = user;
                    ViewBag.UserId = user.Id;
                }
                ViewBag.category = _context.Category.ToList();
                ViewBag.products = _context.Product.ToList();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }

        }
        //[Authorize]
        public async Task<JsonResult> AddToCart(int proId, int qty)
        {
            try
            {
                string userId = _session.GetString("UserId");
                ApplicationUser userApp = new ApplicationUser();
                userApp.Id = userId;
                var user = await _userManager.GetUserIdAsync(userApp);
                if (user == null)
                {
                    return Json(new { redirectToUrl = Url.Action("Login", "Account") });
                }
                else
                {
                    var cartp = _context.Cart.Where(x => x.ProductId == proId).FirstOrDefault();
                    if (cartp != null)
                    {
                        var p = _context.Product.Where(x => x.Id == proId).SingleOrDefault();
                        cartp.ProductId = p.Id;
                        cartp.ProductName = p.Name;
                        cartp.Price = p.UnitPrice;
                        cartp.Discount = Convert.ToDecimal(p.Discount);
                        cartp.Qty += Convert.ToInt32(qty);
                        cartp.Bill = Convert.ToDecimal((cartp.Price * cartp.Qty) - ((cartp.Price * cartp.Qty) * (cartp.Qty * p.Discount) / 100));
                        cartp.PicturePath = p.PicturePath;
                        cartp.CustomerID = userId;

                        _context.Cart.Update(cartp);
                        _context.SaveChanges();
                        if (ViewBag.cart == null)
                        {
                            cartlist.Add(cartp);
                            ViewBag.cart = cartp;
                        }

                        var wl = _context.Wishlist.Where(x => x.ProductID == proId).FirstOrDefault();
                        if (wl != null)
                        {
                            _context.Wishlist.Remove(wl);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        var p = _context.Product.Where(x => x.Id == proId).SingleOrDefault();
                        Cart cart = new Cart();
                        cart.ProductId = p.Id;
                        cart.ProductName = p.Name;
                        cart.Price = p.UnitPrice;
                        cart.Discount = Convert.ToDecimal(p.Discount);
                        cart.Qty = Convert.ToInt32(qty);
                        cart.Bill = Convert.ToDecimal((cart.Price * qty) - ((cart.Price * qty) * (qty * p.Discount) / 100));
                        cart.PicturePath = p.PicturePath;
                        cart.CustomerID = userId;

                        _context.Cart.Add(cart);
                        _context.SaveChanges();
                        if (ViewBag.cart == null)
                        {
                            cartlist.Add(cart);
                            ViewBag.cart = cart;
                        }
                    }
                    return Json(cartlist.Count);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }

        }

        public JsonResult AddToWishList(int proId)
        {
            try
            {
                string userId = _session.GetString("UserId");

                var user = _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    ViewBag.User = user;
                }
                var wlp = _context.Wishlist.Where(x => x.ProductID == proId).FirstOrDefault();
                if (wlp != null)
                {
                    var p = _context.Product.Where(x => x.Id == proId).SingleOrDefault();


                    wlp.CustomerID = userId;
                    //wlp.CustomerID = 0;
                    wlp.ProductID = p.Id;
                    _context.Wishlist.Update(wlp);
                    _context.SaveChanges();
                }
                else
                {
                    var p = _context.Product.Where(x => x.Id == proId).SingleOrDefault();
                    Wishlist wl = new Wishlist();
                    //wl.CustomerID = 0;
                    wl.CustomerID = userId;
                    wl.ProductID = p.Id;
                    _context.Wishlist.Add(wl);
                    _context.SaveChanges();
                }

                return Json("Added to your WishList");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }

        }
        public IActionResult removefromWishtList(int proId)
        {
            try
            {
                var wl = _context.Wishlist.Where(x => x.ProductID == proId).FirstOrDefault();
                var item = _context.Wishlist.Remove(wl);
                _context.SaveChanges();
                return Json("Deleted from Wishlist");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }
        }

        public JsonResult RemoveToCart(int Id)
        {
            try
            {
                var cartitem = _context.Cart.Where(x => x.Id == Id).FirstOrDefault();
                var item = _context.Cart.Remove(cartitem);
                _context.SaveChanges();
                return Json("Deleted from cart");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> AboutUs()
        {
            string userId = _session.GetString("UserId");

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                ViewBag.UserId = user.Id;
            }
            return View();
        }

        public async Task<IActionResult> FAQs()
        {
            string userId = _session.GetString("UserId");

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                ViewBag.UserId = user.Id;
            }
            return View();
        }

        public async Task<IActionResult> ContactUs()
        {
            string userId = _session.GetString("UserId");

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                ViewBag.UserId = user.Id;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}