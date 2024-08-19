using BookStore.Dtos;

namespace BookStore.Services.IServices
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartDto> GetCartByUserIdAsync(string userId);
        Task AddItemToCartAsync(string userId, AddCartItemDto addCartItemDto);
        Task RemoveItemFromCartAsync(string userId, int cartItemId);
        Task ClearCartAsync(string userId);
    }
}
