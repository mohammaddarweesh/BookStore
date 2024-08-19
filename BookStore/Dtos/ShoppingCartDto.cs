namespace BookStore.Dtos
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItemDto> Items { get; set; }
    }
}
