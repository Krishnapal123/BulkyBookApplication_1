using BulkyBookApp.Data;
using BulkyBookApp.Models;
using BulkyBookApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Temporary cart storage; replace with session or database for persistence
        private static Cart _cart = new Cart();

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display the checkout page
       
        public IActionResult Checkout()
        {
            // Create a new instance of the CheckoutViewModel with the current cart
            var model = new CheckoutViewModel
            {
                Cart = _cart // Pass the current cart
            };

            return View(model);
        }

        // Process the checkout and create an order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessCheckout(CheckoutViewModel model)
        {
            Console.WriteLine($"Shipping Address from model: '{model.ShippingAddress}'");

            if (!_cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            // Check if the ModelState is valid before processing
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Model State Error: {error.ErrorMessage}");
                }
                TempData["Error"] = "Please correct the errors and try again.";
                model.Cart = _cart; // Pass the cart back to the view model
                return View("Checkout", model);
            }

            // Create new order
            var newOrder = new Order
            {
                UserId = "Guest", // Replace with actual user ID if available
                TotalAmount = _cart.TotalAmount,
                ShippingAddress = model.ShippingAddress, // Prevent null values
                PaymentStatus = "Pending", // Set initial payment status
                OrderItems = _cart.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price ?? 0,
                    Quantity = item.Quantity
                }).ToList()
            };

            // Save the order to the database
            try
            {
                // Log or debug to check ShippingAddress value
                Console.WriteLine($"Shipping Address: '{model.ShippingAddress}'");

                _context.Orders.Add(newOrder);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error placing the order: " + ex.Message;
                return RedirectToAction("Checkout");
            }

            // Clear the cart after checkout
            _cart.Items.Clear();

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("Index", "Product");
        }

        // Display the cart items
        public IActionResult Index()
        {
            return View(_cart);
        }

        // Add to cart Action
        public IActionResult AddToCart(int id)
        {
            // Simulate retrieving a product from the database
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product != null)
            {
                var existingItem = _cart.Items.FirstOrDefault(item => item.ProductId == product.ProductId);

                if (existingItem != null)
                {
                    existingItem.Quantity++; // Increase quantity if item already in cart
                }
                else
                {
                    // Add new item to cart
                    _cart.Items.Add(new CartItem
                    {
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = 1
                    });
                }

                TempData["Message"] = "Product added to cart successfully!";
            }
            else
            {
                TempData["Error"] = "Product not found!";
            }

            return RedirectToAction("Index", "Cart");
        }

        // Remove item from cart
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var item = _cart.Items.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                _cart.Items.Remove(item);
            }

            return RedirectToAction(nameof(Index));
        }

        // Increase item quantity in cart
        public IActionResult IncreaseQuantity(int id)
        {
            var item = _cart.Items.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                item.Quantity++; // Increment the quantity
            }

            return RedirectToAction(nameof(Index));
        }

        // Decrease item quantity in cart
        public IActionResult DecreaseQuantity(int id)
        {
            var item = _cart.Items.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--; // Decrement the quantity
                }
                else
                {
                    _cart.Items.Remove(item); // Remove item if quantity is 1 and user tries to decrement
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // Place Order Action
        [HttpPost]
        public IActionResult PlaceOrder()
        {
            // Check if the cart has items
            if (!_cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty!";
                return RedirectToAction("Index");
            }

            // Create a new order
            var order = new Order
            {
                CustomerName = "John Doe", // Replace with actual customer data if available
                Address = "123 Main St, City, Country", // Replace with actual address
                TotalAmount = _cart.TotalAmount,
                ShippingAddress = "",
                UserId = "1",
                OrderItems = _cart.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price ?? 0,
                    Quantity = item.Quantity
                }).ToList()
            };

            // Save the order to the database
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Clear the cart after placing the order
            _cart.Items.Clear();

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

       // Display order confirmation page
        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}