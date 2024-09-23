namespace BulkyBookApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; } // Optional: Use this if you want to tie orders to specific users
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string CustomerName { get; set; } // Customer's name (you can add more fields as needed)

        public string Address { get; set; } // Shipping address
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string ShippingAddress { get; set; }
        public string PaymentStatus { get; set; } = "Pending"; // e.g., Paid, Pending, etc.
    }
}
