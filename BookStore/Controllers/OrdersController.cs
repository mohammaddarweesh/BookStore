using BookStore.Dtos;
using BookStore.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _contextAccessor;

        public OrdersController(IOrderService orderService, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _orderService = orderService;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] ProcessOrderDto processOrderDto)
        {
            var order = await _orderService.ProcessOrderAsync(processOrderDto);
            return Ok(order);
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> GetCheckoutDetails()
        {
            var checkoutDetails = await _orderService.GetCheckoutDetailsAsync();
            return Ok(checkoutDetails);
        }
    }
}
