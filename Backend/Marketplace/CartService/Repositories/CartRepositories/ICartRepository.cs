using CartService.Models;

namespace CartService.Repositories.CartRepositories
{
    public interface ICartRepository
    {
        Task<CartProduct?> GetProductByProductIdAsync(string productId, string userId);
        Task<CartProduct?> GetProductAsync(string id);
        Task<List<CartProduct>> GetUserCartProductsAsyn(string userId);
        Task AddCartProductAsync(CartProduct product);
        Task<bool> UpdateCountCartProductAsync(string id, int count);
        Task<bool> RemoveCartProductAsync(string id);
    }
}
