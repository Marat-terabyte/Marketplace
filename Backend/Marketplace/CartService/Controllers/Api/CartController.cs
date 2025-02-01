using CartService.Models;
using CartService.Models.InputModels;
using CartService.Repositories.CartRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;
using System.Text.Json;

namespace CartService.Controllers.Api
{
    [ApiController]
    [Route("/api/cart")]
    [Authorize(Roles = "consumer")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cart;
        private readonly HttpClient _httpClient;
        private readonly Config _config;

        public CartController(ICartRepository cart, HttpClient http, Config config)
        {
            _cart = cart;
            _httpClient = http;
            _config = config;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUserProducts(int from, int to)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var products = await _cart.GetUserCartProductsAsyn(userId);

            if (products == null || products.Count == 0) 
                return NotFound();

            if (products.Count < to)
                return Ok(products[from..products.Count]);

            return Ok(products[from..to]);
        }

        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GeyProductByUserId(string productId)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            var product = await _cart.GetProductByProductIdAsync(productId, userId);
            if (product == null) 
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddProduct(string productId)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var res = await CheckProductAsync(productId, userId);
            if (res.Item1)
                return Ok(res.ToString());

            Product? product = await GetDataFromProductService(productId);
            if (product == null)
                return NotFound();

            var cartProduct = new CartProduct()
            {
                ConsumerId = userId,
                Count = 1,
                CreatedAt = DateTime.Now,
                PriceOfProduct = product.Price,
                ProductId = productId
            };

            await  _cart.AddCartProductAsync(cartProduct);

            return Ok(new { Id = cartProduct.Id.ToString() });
        }

        /// <summary>
        /// Checks a product by <paramref name="productId"/>
        /// </summary>
        /// <returns><c>true</c> if the product exists, or <c>false</c> if it doesn't</returns>
        private async Task<(bool, ObjectId?)> CheckProductAsync(string productId, string userId)
        {
            ObjectId? cartProductId = null;
            
            var cartProduct = await _cart.GetProductByProductIdAsync(productId, userId);
            if (cartProduct != null)
            {
                cartProductId = cartProduct.Id;

                return (true, cartProductId);
            }

            return (false, null);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProducts(string id, [FromBody] UpdateInput input)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            
            var product = await _cart.GetProductAsync(id);
            if (product == null)
                return NotFound();

            if (product.ConsumerId != userId)
                return Forbid();

            bool isCompleted = await _cart.UpdateCountCartProductAsync(id, input.Count);
            if (!isCompleted)
                return StatusCode(500);

            return Ok();
        }

        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> DeleteProducts(string productId)
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var product = await _cart.GetProductAsync(productId);
            if (product == null) 
                return NotFound();

            if (product.ConsumerId != userId)
                return Forbid();

            bool isCompleted = await _cart.RemoveCartProductAsync(productId);
            if (!isCompleted)
                return StatusCode(500);

            return Ok();
        }

        private async Task<Product?> GetDataFromProductService(string id)
        {
            var baseUrl = new Uri(_config.ProductServiceAddress);
            var uriBuilder = new UriBuilder(baseUrl)
            {
                Path = _config.PathToGetProduct.TrimStart('/'),
                Query = $"id={id}",
            };

            var response = await _httpClient.GetAsync(uriBuilder.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Product? product = JsonSerializer.Deserialize<Product>(json);

            return product;
        }
    }
}
