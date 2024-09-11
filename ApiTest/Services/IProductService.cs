using ApiTest.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
