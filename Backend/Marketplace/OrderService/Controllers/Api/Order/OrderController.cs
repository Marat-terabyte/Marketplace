using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models.Orders.Repositories;
using System.Security.Claims;

namespace OrderService.Controllers.Api.Order
{
    [ApiController]
    [Route("/api/order")]
    public class OrderController : Controller
    {
        private IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [Route("consumer")]
        [Authorize(Roles = "consumer")]
        public async Task<IActionResult> GetByConsumerId(int from, int to)
        {
            string? id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
                return Unauthorized();

            var orders = await _orderRepository.GetOrdersByConsumerIdAsync(id);
            if (orders == null || orders.Count == 0)
                return NotFound();

            if (to >= orders.Count)
                return Ok(orders[from..orders.Count]);

            return Ok(orders[from..to]);
        }

        [HttpGet]
        [Route("seller")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> GetBySellerId(int from, int to)
        {
            string? id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
                return Unauthorized();

            var orders = await _orderRepository.GetOrdersBySellerIdAsync(id);
            if (orders == null || orders.Count == 0)
                return NotFound();

            if (to >= orders.Count)
                return Ok(orders[from..orders.Count]);

            return Ok(orders[from..to]);
        }

        [HttpPut]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> UpdateDeliveryStatus(string orderId, bool isDelivered)
        {
            string? id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
                return Unauthorized();

            bool isSucces = await _orderRepository.UpdateDeliveryStatus(id, orderId, isDelivered);
            if (isSucces)
                return Ok();

            return BadRequest();
        }

    }
}
