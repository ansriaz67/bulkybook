using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
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
                _db.RegisterModel.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "User Registered Successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
