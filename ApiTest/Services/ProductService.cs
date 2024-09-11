using ApiTest.Contracts.Models;
using ApiTest.Entity.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Services
{
    /// <summary>
    /// Provides methods for managing products, including retrieving, adding, and updating products in the database.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The product database context used for data access.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided context is <see langword="null"/>.</exception>
        public ProductService(ProductDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the product if found;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        /// <summary>
        /// Retrieves all products, with an optional filter for products updated after a specific date.
        /// </summary>
        /// <param name="updatedAfter">Filters products updated after the specified date (optional).</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of products.
        /// </returns>
        /// <remarks>
        /// If the <paramref name="updatedAfter"/> parameter is provided, only products with a 
        /// <see cref="Product.LastUpdated"/> date later than the specified value will be returned.
        /// </remarks>
        public async Task<IEnumerable<Product>> GetAllProductsAsync(DateTime? updatedAfter = null)
        {
            var query = _context.Products.AsQueryable();

            if (updatedAfter.HasValue)
            {
                query = query.Where(p => p.LastUpdated > updatedAfter.Value);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// This method adds the product to the database context and saves the changes to persist the new product.
        /// </remarks>
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="product">The product to update with new values.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// The method updates the product in the database context and saves the changes to persist the update.
        /// </remarks>
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
