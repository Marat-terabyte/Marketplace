using MongoDB.Bson;

namespace ProductService.Models.Products.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<Product?> GetAsync(string id);
        Task<List<Product>> GetBySellerIdAsync(string sellerId, int from, int to);
        Task DeleteAsync(string id);
        Task UpdateAsync(Product updatedProduct);
    }
}
