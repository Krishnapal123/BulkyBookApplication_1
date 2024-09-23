using BulkyBookApp.Data;
using BulkyBookApp.Models;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;

namespace BulkyBookApp.Controllers
{
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult UserIndex()
        {

            IEnumerable<Userclass> objUserclassList = _context.User;
           
            return View(objUserclassList);
        }

        // GET
        public IActionResult UserCreate()
        {

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserCreate(Userclass obj)
        {
            if (obj.Username == obj.Password?.ToString())
            {
                ModelState.AddModelError("UserError", "The Display order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                byte[] key = Common.Encryption.GenerateKey();

                byte[] iv = Common.Encryption.GenerateIV();
                obj.Password = Common.Encryption.Encrypt(obj.Password, key, iv);

                _context.User.Add(obj);
                _context.SaveChanges();
                TempData["Success"] = "User created Successfully";
                return RedirectToAction("UserIndex");
            }
            return View(obj);
        }


        // GET
        public IActionResult UserEdit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var UserclassFromDb = _context.User.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id == id);
            //var categoryFromSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);

            if (UserclassFromDb == null)
            {
                return NotFound();
            }
            return View(UserclassFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserEdit(Userclass obj)
        {
            if (obj.Username == obj.Password?.ToString())
            {
                ModelState.AddModelError("UserError", "The Display order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _context.User.Update(obj);
                _context.SaveChanges();
                TempData["Success"] = "User updated Successfully";
                return RedirectToAction("UserIndex");
            }
            return View(obj);
        }

        //GET

        public IActionResult UserDelete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var UserclassFromDb = _context.User.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id == id);
            //var categoryFromSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);

            if (UserclassFromDb == null)
            {
                return NotFound();
            }
            return View(UserclassFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            var obj = _context.User.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _context.User.Remove(obj);
            _context.SaveChanges();
            TempData["Success"] = "User deleted Successfully";
            return RedirectToAction("UserIndex");
        }
    }
}
