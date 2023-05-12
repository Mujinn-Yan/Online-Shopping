using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStoreForWoman.DataAccess;
using OnlineStoreForWoman.DTOs;
using OnlineStoreForWoman.Models;

namespace OnlineStoreForWoman.Controllers
{
    public class ViewCartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        List<Cart> cartlist = new List<Cart>();
        private readonly ApplicationDbContext _context;
        private ISession _session;
        private readonly UserManager<ApplicationUser> _userManager;

        public ViewCartController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)

        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _session = httpContextAccessor.HttpContext.Session;

        }

        public async Task<IActionResult> ViewCart(string customerID)
        {
            try
            {
                string userId = _session.GetString("UserId");
                ApplicationUser userApp = new ApplicationUser();
                userApp.Id = userId;
                var user = await _userManager.GetUserIdAsync(userApp);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else if(user != null)
                {
                    ViewBag.User = user;
                    ViewBag.UserId = userId;
                }

                var cart = _context.Cart.Where(x => x.CustomerID == userId).ToList();
                ViewBag.CartItems = cart;
                ViewBag.TotalAmount = cart.Sum(x => x.Bill);
                ViewBag.TotalDiscount = cart.Sum(x => x.Discount);
                ViewBag.CustomerId = userId;

                return View(ViewBag.CartItems);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToAction("/Shared/Error");
            }

        }
        
        [HttpPost]
        public JsonResult UpdateCart(Cart[] cartItem, string customerId)
        {
            try
            {
                string userId = _session.GetString("UserId");
                ViewBag.CustomerId = userId;
                //ApplicationUser userApp = new ApplicationUser();
                //userApp.Id = userId;
                //var user = await _userManager.GetUserIdAsync(userApp);
                //if (user == null)
                //{
                //    return Json(new { redirectToUrl = Url.Action("Login", "Account") });
                //}
                foreach (var item in cartItem)
                {
                    var p = _context.Cart.Where(x => x.ProductId == item.ProductId && x.CustomerID == userId).FirstOrDefault();
                    if (p != null)
                    {
                        p.ProductId = item.ProductId;
                        p.ProductName = item.ProductName;
                        p.Price = item.Price;
                        p.Discount = item.Discount;
                        p.Qty = item.Qty;
                        p.Bill = item.Price * Convert.ToInt32(item.Qty);
                        p.PicturePath = item.PicturePath.Remove(0, 1);
                        _context.Cart.Update(p);
                        _context.SaveChanges();
                    }
                }
                return Json("Updated Cart Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }

        }

        public async Task<IActionResult> Checkout(string customerID)
        {
            try
            {
                string userId = _session.GetString("UserId");
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    ViewBag.UserId = user.Id;
                }

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                else
                {
                    var cart = _context.Cart.Where(x => x.CustomerID == userId).ToList();
                    ViewBag.CartCount = cart.Count();
                    ViewBag.TotalAmount = cart.Sum(x => x.Bill);
                    ViewBag.TotalDiscount = cart.Sum(x => x.Discount);
                    ViewBag.User = user;
                    ViewBag.CustomerId = userId;

                    return View();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckoutOrder(CheckOutDtoNew data)
        {
            try
            {
                if (data != null)
                {
                    string userId = _session.GetString("UserId");
                    ApplicationUser userApp = new ApplicationUser();
                    userApp.Id = userId;
                    var user = await _userManager.FindByIdAsync(userApp.Id);
                    if (user == null)
                    {
                        //return RedirectToAction("Login", "Account");
                    }

                    var cart = _context.Cart.Where(x => x.CustomerID == userId).ToList();
                    //ViewBag.CartItems = cart;
                    ViewBag.TotalAmount = cart.Sum(x => x.Bill);
                    ViewBag.TotalDiscount = cart.Sum(x => x.Discount);
                    ViewBag.CustomerId = userId;

                    //add order
                    var order = new Order();
                    order.CustomerID = user.Id;
                    order.ShippingID = 1;
                    order.Discount = Convert.ToInt32(ViewBag.TotalDiscount);
                    order.Taxes = 0;
                    order.TotalAmount = Convert.ToInt32(ViewBag.TotalAmount);
                    order.isCompleted = true;
                    order.OrderDate = DateTime.Now;
                    order.DeliveryDate = DateTime.Now;
                    order.DIspatched = false;
                    order.Shipped = false;
                    order.ShippingDate = DateTime.Now;
                    order.Deliver = false;
                    order.DeliveryDate = DateTime.Now;
                    order.Notes = "";
                    order.CancelOrder = false;
                    _context.Order.Add(order);
                    _context.SaveChanges();


                    foreach (var item in cart)
                    {
                        var orderDetails = new OrderDetail();
                        orderDetails.OrderID = order.Id;
                        orderDetails.ProductID = item.ProductId;
                        orderDetails.UnitPrice = item.Price;
                        orderDetails.Quantity = item.Qty;
                        orderDetails.Discount = item.Discount;
                        orderDetails.TotalAmount = item.Bill;
                        orderDetails.OrderDate = DateTime.Now;
                        _context.OrderDetail.Add(orderDetails);
                        _context.SaveChanges();
                    }

                    //add shipping details
                    var shipping = new ShippingDetail();
                    shipping.OrderId = order.Id;
                    shipping.FirstName = data.FirstName;
                    shipping.LastName = data.LastName;
                    shipping.Email = data.Email;
                    shipping.Mobile = data.Mobile;
                    shipping.City = data.City;
                    shipping.Address = data.City;
                    shipping.PostCode = data.PCode;
                    _context.ShippingDetail.Add(shipping);
                    _context.SaveChanges();

                    var cartitem = _context.Cart.Where(x => x.CustomerID == userId).ToList();
                    if(cartitem != null)
                    {
                        _context.RemoveRange(cartitem);
                        _context.SaveChanges();
                    }

                }
                return Json("Order submitted Successfully");
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }

        public IActionResult MyWishList(string customerID)
        {
            try
            {
                string userId = _session.GetString("UserId");
                var user = _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    ViewBag.User = user;
                    ViewBag.UserId = user.Id;
                }
                var wl = _context.Wishlist.Include(x => x.Product).Where(x => x.CustomerID == customerID).ToList();
                ViewBag.WishList = wl;
                ViewBag.CustomerId = userId;

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(ex.ToString());
            }

        }

    }
}
