namespace BulkyBookApp.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; } // Unique identifier for cart item
        public int ProductId { get; set; }  // Foreign key to Product
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; } // Quantity of the product in the cart
        public decimal TotalPrice => (Price ?? 0) * Quantity; // Use 0 if Price is null
    }
}
