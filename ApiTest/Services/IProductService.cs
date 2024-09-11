using ApiTest.Contracts.Models;

namespace ApiTest.Services
{
    public interface IProductService
    {
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync(DateTime? updatedAfter = null);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
    }
}
