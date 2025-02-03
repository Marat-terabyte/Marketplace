using CartService.Models.InputModels;
using CartService.Repositories.CartRepositories;
using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CartService.Controllers.Api
{
    [ApiController]
    [Route("/api/cart/buy")]
    [Authorize(Roles = "consumer")]
    public class BuyController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMsgBroker _msgBroker;

        public BuyController(ICartRepository cartRepository, IMsgBroker msgBroker)
        {
            _cartRepository = cartRepository;
            _msgBroker = msgBroker;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> BuySelectedProducts([FromBody] BuyInput input)
        {
            // TODO: Implement 'BuySelectedProducts' method
            BuyTransactionModel model = await CreateTransactionModelAsync(input);
            if (model.ProductIds.Count == 0)
                return NotFound();

            string json = JsonSerializer.Serialize(model);
            await _msgBroker.SendMessageAsync("", "decrease_balance_queue", json);

            foreach (var selectedProduct in input.SelectedCartProducts)
            {
                await _cartRepository.RemoveCartProductAsync(selectedProduct.ProductCartId);
            }

            return Ok();
        }

        private async Task<BuyTransactionModel> CreateTransactionModelAsync(BuyInput input)
        {
            BuyTransactionModel model = new BuyTransactionModel();

            foreach (var selectedProduct in input.SelectedCartProducts)
            {
                var product = await _cartRepository.GetProductAsync(selectedProduct.ProductCartId);
                if (product == null)
                    continue;

                model.SellerIds.Add(product.SellerId);
                model.ConsumerIds.Add(product.ConsumerId);
                model.ProductIds.Add(product.ProductId);
                model.Counts.Add(product.Count);
                model.Prices.Add(product.PriceOfProduct);
            }

            model.DeliveryPlace = input.DeliveryPlace;

            return model;
        }
    }
}
