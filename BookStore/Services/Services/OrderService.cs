using AutoMapper;
using BookStore.Dtos;
using BookStore.Models;
using BookStore.Repositories.IRepositories;
using BookStore.Services.IServices;
using System.Security.Claims;

namespace BookStore.Services.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(
            IOrderRepository orderRepository,
            IShoppingCartRepository shoppingCartRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetUserIdFromToken()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<OrderDto> ProcessOrderAsync(ProcessOrderDto processOrderDto)
        {
            var userId = GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var cart = await _shoppingCartRepository.GetByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Shopping cart is empty.");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = processOrderDto.ShippingAddress,
                TotalAmount = cart.Items.Sum(i => i.Quantity * i.Book.Price),
                Items = cart.Items.Select(i => new OrderItem
                {
                    BookId = i.BookId,
                    Quantity = i.Quantity,
                    Price = i.Book.Price
                }).ToList()
            };

            // Create the order in the database
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Update book availability
            foreach (var item in cart.Items)
            {
                await _orderRepository.UpdateBookAvailabilityAsync(item.BookId);
            }

            // Clear the shopping cart
            await _shoppingCartRepository.ClearCartAsync(cart.Id);

            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task<OrderDto> GetCheckoutDetailsAsync()
        {
            var userId = GetUserIdFromToken();
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var cart = await _shoppingCartRepository.GetByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Shopping cart is empty.");
            }

            var orderDetails = new OrderDto
            {
                OrderDate = DateTime.UtcNow,  // or leave it as default for now
                TotalAmount = cart.Items.Sum(i => i.Quantity * i.Book.Price),
                Items = cart.Items.Select(i => new OrderItemDto
                {
                    BookId = i.BookId,
                    Quantity = i.Quantity,
                    Price = i.Book.Price
                }).ToList()
            };

            return orderDetails;
        }
    }
}
