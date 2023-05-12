using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStoreForWoman.DataAccess;
using OnlineStoreForWoman.Models;

namespace OnlineStoreForWoman.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ISession _session;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _session = httpContextAccessor.HttpContext.Session;

        }

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
                var order = _context.Order
     .Include(x => x.OrderDetails)
     .Include(x => x.ShippingDetail)
     .Select(x => new Order
     {
         CustomerID = x.CustomerID,
         CustomerName = _context.Users.Where(u => u.Id == x.CustomerID).Select(u => u.UserName).FirstOrDefault(),
         //ShippingID = x.ShippingID,
         Discount = x.Discount,
         Taxes = x.Taxes,
         TotalAmount = x.TotalAmount,
         isCompleted = x.isCompleted,
         OrderDate = x.OrderDate,
         //DIspatched = x.DIspatched,
         DispatchedDate = x.DispatchedDate,
         //Shipped = x.Shipped,
         ShippingDate = x.ShippingDate,
         //Deliver = x.Deliver,
         DeliveryDate = x.DeliveryDate,
         Notes = x.Notes,
         //CancelOrder = x.CancelOrder,
         OrderDetails = x.OrderDetails,
         ShippingDetail = x.ShippingDetail
     })
     .ToList();

                return View(order);
            }
        }
    }
}
