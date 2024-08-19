using BookStore.Data;
using BookStore.Models;
using BookStore.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repositories.Repositories
{
    public class ShoppingCartRepository : BaseRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> GetByUserIdAsync(string userId)
        {
            return await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Book)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(int cartItemId)
        {
            var item = await _context.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int shoppingCartId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == shoppingCartId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.Items);
                await _context.SaveChangesAsync();
            }
        }
    }
}
