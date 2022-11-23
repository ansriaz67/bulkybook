using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;

        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public IActionResult Index(LoginModel obj)
        {
           
            if (ModelState.IsValid)
            {
                var data = _db.RegisterModel.Where(e=>e.Email== obj.Email).SingleOrDefault();
                if(data != null)
                {
                   
                    bool isValid = (data.Email == obj.Email && data.Password == obj.Password);
                    if(isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, obj.Email) }, CookieAuthenticationDefaults.AuthenticationScheme );
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Email", obj.Email);
                        HttpContext.Session.SetString("Password", obj.Password);
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        TempData["error"] = "You have entered the wrong password.";
                        return View();
                    }
                }
                else
                {
                    TempData["error"] = "Something went wrong.";
                    return View();
                }

            }
            TempData["error"] = "Something went wrong.";
            return View();
        }

        public IActionResult Signout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel obj)
        {
          
            if (ModelState.IsValid)
            {
                var isEmailAlreadyExists = _db.RegisterModel.Any(x => x.Email == obj.Email);
                if (isEmailAlreadyExists)
                {
                    ModelState.AddModelError("Email", "User with this email already exists");
                    return View(obj);
                }
                _db.RegisterModel.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "User Registered Successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
