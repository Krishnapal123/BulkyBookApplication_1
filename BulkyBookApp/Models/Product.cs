using BulkyBookWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace BulkyBookApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? Image { get; set; } = "";
      
        public decimal? Price { get; set; }
        public int Quantity { get; set; }

        // New property for Category (assuming it's stored as an int ID)
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        // Navigation property if using EF Core for related data
        public Category? Category { get; set; }

    }
}
