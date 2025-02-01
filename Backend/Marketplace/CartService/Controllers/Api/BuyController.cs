using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers.Api
{
    [ApiController]
    [Route("/api/cart/buy")]
    public class BuyController : Controller
    {
        [HttpPost]
        [Route("")]
        public IActionResult BuySelectedProducts()
        {
            // TODO: Implement 'BuySelectedProducts' method
            throw new NotImplementedException();
        }
    }
}
