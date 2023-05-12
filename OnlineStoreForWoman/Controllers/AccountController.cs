using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using OnlineStoreForWoman.Data;
using OnlineStoreForWoman.DataAccess;
using OnlineStoreForWoman.DTOs;
using OnlineStoreForWoman.Entities;
using OnlineStoreForWoman.Extensions;
using OnlineStoreForWoman.Helpers;
using OnlineStoreForWoman.Models;
using OnlineStoreForWoman.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace OnlineStoreForWoman.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly ISession _session;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _session = httpContextAccessor.HttpContext.Session;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    //var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    //var token = new JwtSecurityToken(
                    //    issuer: _configuration["JWT:ValidIssuer"],
                    //    audience: _configuration["JWT:ValidAudience"],
                    //    expires: DateTime.Now.AddHours(3),
                    //    claims: authClaims,
                    //    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    //);

                    //var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    //// Save token to session
                    //HttpContext.Session.SetString("JWTToken", tokenString);
                    HttpContext.Session.SetString("UserId", user.Id);

                    // Create a ClaimsIdentity object with the user's claims
                    //var claimsIdentity = new ClaimsIdentity(authClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    //// Set the current user's identity to the ClaimsIdentity object
                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (authClaims[0].Value == "admin")
                    {
                        //RedirectToAction("Index", "Home", new { area = "Admin" });
                        return Json("Admin");
                        //return Json(new { redirectToUrl = Url.Action("Index", "Home", new { Area = "Admin" }), token = "123445" });
                    }
                    else
                    {
                        return Json("User");
                        //return Json(new { redirectToUrl = Url.Action("Index", "Home"), token = "123445" });
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var model = new Register();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                {
                    ModelState.AddModelError("", "User already exists!");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Create the "User" role if it doesn't already exist
                    var roleExists = await roleManager.RoleExistsAsync("User");
                    if (!roleExists)
                    {
                        await roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    // Assign the user to the "User" role
                    await userManager.AddToRoleAsync(user, "User");

                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

         
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return View("Index");
        }


    }
}
