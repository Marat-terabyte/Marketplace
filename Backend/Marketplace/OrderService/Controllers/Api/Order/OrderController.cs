using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Servers;
using OrderService.Models.Orders;
using OrderService.Models.Orders.Repositories;
using System.Security.Claims;
using System.Text.Json;

namespace OrderService.Controllers.Api.Order
{
    [ApiController]
    [Route("/api/order")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMsgBroker _msgBroker;

        public OrderController(IOrderRepository orderRepository, IMsgBroker msgBroker)
        {
            _msgBroker = msgBroker;
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
            {
                // Send notification to a consumer
                if (isDelivered)
                {
                    Models.Orders.Order? order = await _orderRepository.GetOrderAsync(orderId);
                    await _msgBroker.SendMessageAsync("", "notification_queue", JsonSerializer.Serialize<Notification>(CreateNotification(order!.ConsumerId, orderId)));
                }
                return Ok();
            }
            return BadRequest();
        }

        private Notification CreateNotification(string consumerId, string productId)
        {
            Notification notification = new Notification()
            {
                UserId = consumerId,
                Message = $"Delivered {productId}"
            };

            return notification;
        }
    }
}
