using BulkyBookApp.Data;
using BulkyBookApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _pro;

        public ProductController(ApplicationDbContext pro)
        {
            _pro = pro;
        }

        //// GET: ProductIndex
        //public IActionResult ProductIndex()
        //{
        //    // Include the Category navigation property to load category data with each product
        //    IEnumerable<Product> objProductclassList = _pro.Products.Include(p => p.Category).ToList();

        //    return View(objProductclassList);
        //}
        // GET: ProductIndex
        public IActionResult ProductIndex()
        {
            // Fetch products without including categories
            var products = _pro.Products.ToList();

            // Fetch categories separately
            var categories = _pro.Categories.ToList();

            // Manually map categories to products
            foreach (var product in products)
            {
                product.Category = categories.FirstOrDefault(c => c.Id == product.CategoryId);
            }

            return View(products);
        }



        // GET
        public IActionResult ProductCreate()
        {
            // Populate the ViewBag with categories
            ViewBag.Categories = GetCategories();
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProductCreate(Product obj)
        {
            if (obj.ProductId.ToString() == obj.Name)

            {
                ModelState.AddModelError("UserError", "The Display order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _pro.Products.Add(obj);
                _pro.SaveChanges();
                TempData["Success"] = "Product created Successfully";
                return RedirectToAction("ProductIndex");
            }
            // Repopulate categories if validation fails
            ViewBag.Categories = GetCategories();
            return View(obj);
        }

        // GET
        // GET: Product/Edit/5
        public IActionResult ProductEdit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = _pro.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            // Populate the ViewBag with categories for the edit form
            ViewBag.Categories = GetCategories();
            // Return the view with the product data
            return View(product);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProductEdit(Product obj)
        {
            if (ModelState.IsValid)
            {
                // Find the existing product in the database
                var existingProduct = _pro.Products.FirstOrDefault(p => p.ProductId == obj.ProductId);
                if (existingProduct != null)
                {
                    // Update the existing product's properties
                    existingProduct.Name = obj.Name;
                    existingProduct.Image = obj.Image;
                    existingProduct.Price = obj.Price;
                    existingProduct.Quantity = obj.Quantity;
                    existingProduct.CategoryId = obj.CategoryId;

                    // Save changes to the database
                    _pro.Products.Update(existingProduct);
                    _pro.SaveChanges();

                    TempData["Success"] = "Product updated Successfully";
                    return RedirectToAction("ProductIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Product not found.");
                }
            }

            // Repopulate categories if validation fails
            ViewBag.Categories = GetCategories();
            return View(obj);
        }


        //GET

        public IActionResult ProductDelete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ProductFromDb = _pro.Products.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id == id);
            //var categoryFromSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);

            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? ProductId)
        {
            var obj = _pro.Products.Find(ProductId);
            if (obj == null)
            {
                return NotFound();
            }
            _pro.Products.Remove(obj);
            _pro.SaveChanges();
            TempData["Success"] = "Product deleted Successfully";
            return RedirectToAction("ProductIndex");
        }

        // GET: GetCategories
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _pro.Categories
                .Select(c => new { value = c.Id.ToString(), text = c.Name })
                .ToList();
            return Json(categories);
        }

        // Helper method to get categories
        //private List<SelectListItem> GetCategoriesFromDatabase()
        //{
        //    return _pro.Categories.Select(c => new SelectListItem
        //    {
        //        Value = c.Id.ToString(),
        //        Text = c.Name
        //    }).ToList();
        //}

    } 
}
