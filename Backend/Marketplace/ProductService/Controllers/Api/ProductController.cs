using Marketplace.Shared.Formatters;
using Marketplace.Shared.Models;
using Marketplace.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models.InputModels;
using ProductService.Models.Products;
using ProductService.Services;
using System.Security.Claims;
using System.Text.Json;

namespace ProductService.Controllers.Api
{
    [ApiController]
    [Route("/api/products")]
    public class ProductController : Controller
    {
        private ItemService _productService;

        public ProductController(ItemService productService)
        {
            _productService = productService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            Product? product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [Route("seller/")]
        [HttpGet]
        public async Task<IActionResult> GetBySellerId(string sellerId, int from, int to)
        {
            List<Product> products = await _productService.GetBySellerIdAsync(sellerId, from, to);
            if (products.Count == 0)
                return NotFound();

            return Ok(products);
        }

        [Authorize(Roles = "seller")]
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorFormater.Create(ModelState));
            }

            string? storename = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            string? identifier = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (identifier == null || storename == null)
                return Unauthorized();

            Product product = new Product()
            {
                Name = input.Name,
                Description = input.Description,
                SellerId = identifier,
                Storename = storename,
                Category = input.Category,
                Price = input.Price,
                Stock = input.Stock,
                Images = input.Images,
                Attributes = input.Attributes,
                CreatedAt = DateTime.Now,
            };

            await _productService.AddItemAsync(product);
            
            return Ok(new { Id = product.Id.ToString() });
        }

        [Authorize(Roles = "seller")]
        [Route("")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorFormater.Create(ModelState));
            }

            Product? product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            string? identifier = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (identifier == null || product.SellerId != identifier)
                return Forbid();

            product.Name = input.Name;
            product.Description = input.Description;
            product.SellerId = identifier;
            product.Category = input.Category;
            product.Price = input.Price;
            product.Stock = input.Stock;
            product.Images = input.Images;
            product.Attributes = input.Attributes;
            product.UpdatedAt = DateTime.Now;

            await _productService.UpdateAsync(product);
         
            return Ok(new { Id = product.Id });
        }

        [Authorize(Roles = "seller")]
        [Route("")]
        [HttpDelete]
        public async Task<IActionResult> RemoveProduct(string id)
        {
            Product? product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            string? identifier = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (identifier == null || product.SellerId != identifier)
                return Forbid();

            await _productService.DeleteAsync(id);

            return Ok();
        }
    }
}
