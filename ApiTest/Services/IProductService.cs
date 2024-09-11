using ApiTest.Contracts.Models;

namespace ApiTest.Services
{
    public interface IProductService
    {
        Task<ProductModel?> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();
        Task AddProductAsync(ProductModel product);
        Task UpdateProductAsync(ProductModel product);
    }
}
