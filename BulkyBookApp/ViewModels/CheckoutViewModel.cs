using BulkyBookApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BulkyBookApp.ViewModels
{
    public class CheckoutViewModel
    {
        public Cart Cart { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        [StringLength(500, ErrorMessage = "Shipping address cannot be longer than 500 characters.")]
        public string ShippingAddress { get; set; }

    }
}
