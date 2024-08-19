namespace BookStore.Dtos
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public string BookTitle { get; set; }
        public decimal BookPrice { get; set; }
    }
}
