using BulkyBookApp.Models;

namespace BulkyBookApp.Views.ViewModels
{
    public class CheckoutViewModel
    {
        public Cart Cart { get; set; } = new Cart();  // Holds cart data
        public string? ShippingAddress { get; set; }  // Holds shipping address data
    }
}
