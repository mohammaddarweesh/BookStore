namespace BookStore.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
    }
}
