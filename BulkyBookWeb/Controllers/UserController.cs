using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BulkyBookWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var userList = from c in _db.RegisterModel
                                    select c;
            return View(userList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RegisterModel obj)
        {
            var chkEmail = _db.RegisterModel.Where(e => e.Email == obj.Email).SingleOrDefault();
            
            if (obj.Password != obj.ConfirmPassword.ToString())
            {
                ModelState.AddModelError("name", "The Password does not match.");
            }
            else if (obj.Email == "" || obj.Name == "" || obj.Password == "" || obj.ConfirmPassword == "")
            {
                ModelState.AddModelError("name", "Field is required.");
            }
            else if (ModelState.IsValid)
            {
                _db.RegisterModel.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "User Created Successfully!";
                return RedirectToAction("Index", "User");
            }
            return View(obj);
        }


        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var userFromDb = _db.RegisterModel.Find(id);
            if (userFromDb == null)
            {
                return NotFound();
            }
            return View(userFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RegisterModel obj)
        {
            if (obj.Password != obj.ConfirmPassword.ToString())
            {
                ModelState.AddModelError("name", "The Password does not match.");
            }
            if (ModelState.IsValid)
            {
                _db.RegisterModel.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "User Updated Successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var userFromDb = _db.RegisterModel.Find(id);
            if (userFromDb == null)
            {
                return NotFound();
            }
            return View(userFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.RegisterModel.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.RegisterModel.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "User Deleted Successfully!";
            return RedirectToAction("Index");
        }
    }
}
