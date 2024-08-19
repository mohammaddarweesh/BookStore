using BookStore.Dtos;
using BookStore.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure that only authenticated users can access these endpoints
    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ShoppingCartController(IShoppingCartService shoppingCartService, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetCurrentUserId();
            var cart = await _shoppingCartService.GetCartByUserIdAsync(userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemDto addCartItemDto)
        {
            var userId = GetCurrentUserId();
            await _shoppingCartService.AddItemToCartAsync(userId, addCartItemDto);
            return NoContent();
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var userId = GetCurrentUserId();
            await _shoppingCartService.RemoveItemFromCartAsync(userId, cartItemId);
            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserId();
            await _shoppingCartService.ClearCartAsync(userId);
            return NoContent();
        }
    }
}
