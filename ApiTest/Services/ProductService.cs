using ApiTest.Contracts.Models;
using ApiTest.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(DateTime? updatedAfter = null)
        {
            var query = _context.Products.AsQueryable();

            if (updatedAfter.HasValue)
            {
                query = query.Where(p => p.LastUpdated > updatedAfter.Value);
            }

            return await query.ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
