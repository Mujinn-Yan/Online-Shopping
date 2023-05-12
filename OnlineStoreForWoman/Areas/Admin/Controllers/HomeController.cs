using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineStoreForWoman.DataAccess;

namespace OnlineStoreForWoman.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ISession _session;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
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

                return View();
            }

        }
    }
}
