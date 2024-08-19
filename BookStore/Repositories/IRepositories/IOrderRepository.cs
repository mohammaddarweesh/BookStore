using BookStore.Models;

namespace BookStore.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task UpdateBookAvailabilityAsync(int bookId);
    }
}
