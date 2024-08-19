namespace BookStore.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Identifies the user who owns the cart
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
