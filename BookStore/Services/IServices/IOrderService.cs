using BookStore.Dtos;

namespace BookStore.Services.IServices
{
    public interface IOrderService
    {
        Task<OrderDto> ProcessOrderAsync(ProcessOrderDto processOrderDto);
        Task<OrderDto> GetCheckoutDetailsAsync();
    }
}
