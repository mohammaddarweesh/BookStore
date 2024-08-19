using BookStore.Models;

namespace BookStore.Repositories.IRepositories
{
    public interface IShoppingCartRepository : IBaseRepository<ShoppingCart>
    {
        Task<ShoppingCart> GetByUserIdAsync(string userId);
        Task AddItemAsync(CartItem cartItem);
        Task RemoveItemAsync(int cartItemId);
        Task ClearCartAsync(int shoppingCartId);
    }
}
