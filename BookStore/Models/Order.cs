namespace BookStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Identifies the user who placed the order
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
