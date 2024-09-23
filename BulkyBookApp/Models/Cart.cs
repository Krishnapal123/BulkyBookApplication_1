namespace BulkyBookApp.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>(); // List of cart items

        public decimal TotalAmount => Items.Sum(item => item.TotalPrice); // Calculate total cart amount
    }
}
