using AutoMapper;
using BookStore.Dtos;
using BookStore.Exceptions;
using BookStore.Models;
using BookStore.Repositories.IRepositories;
using BookStore.Services.IServices;

namespace BookStore.Services.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public ShoppingCartService(
            IShoppingCartRepository shoppingCartRepository,
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<ShoppingCartDto> GetCartByUserIdAsync(string userId)
        {
            var cart = await _shoppingCartRepository.GetByUserIdAsync(userId);
            return _mapper.Map<ShoppingCartDto>(cart);
        }

        public async Task AddItemToCartAsync(string userId, AddCartItemDto addCartItemDto)
        {
            var cart = await _shoppingCartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                await _shoppingCartRepository.AddAsync(cart);
            }

            var book = await _bookRepository.GetByIdAsync(addCartItemDto.BookId);
            if (book == null)
            {
                throw new NotFoundException("Book Not Found");
            }
            var cartItem = new CartItem
            {
                BookId = addCartItemDto.BookId,
                Quantity = addCartItemDto.Quantity,
                Book = book,
                ShoppingCartId = cart.Id
            };

            cart.Items.Add(cartItem);
            await _shoppingCartRepository.AddItemAsync(cartItem);
        }

        public async Task RemoveItemFromCartAsync(string userId, int cartItemId)
        {
            var cart = await _shoppingCartRepository.GetByUserIdAsync(userId);
            if (cart != null)
            {
                var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
                if (item != null)
                {
                    cart.Items.Remove(item);
                    await _shoppingCartRepository.RemoveItemAsync(cartItemId);
                }
                else
                {
                    throw new NotFoundException("Item Not Found");
                }
            }
            else
            {
                throw new NotFoundException("There is no cart for the specified user");
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _shoppingCartRepository.GetByUserIdAsync(userId);
            if (cart != null)
            {
                await _shoppingCartRepository.ClearCartAsync(cart.Id);
            }
            else
            {
                throw new NotFoundException("There is no cart for the specified user");
            }
        }
    }
}
