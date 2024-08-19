using BookStore.Data;
using BookStore.Models;
using BookStore.Repositories.IRepositories;

namespace BookStore.Repositories.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateBookAvailabilityAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.IsAvailable = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
