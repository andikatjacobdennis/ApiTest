using ApiTest.Contracts.Models;

namespace ApiTest.Services
{
    /// <summary>
    /// Provides methods to manage products, including retrieval, creation, and updates.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>
        /// A <see cref="Product"/> if the product is found; otherwise, <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// Example usage:
        /// 
        ///     var product = await _productService.GetProductByIdAsync(id);
        /// </remarks>
        Task<Product?> GetProductByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all products, optionally filtering those updated after a certain date.
        /// </summary>
        /// <param name="updatedAfter">Filters the products to those updated after the specified date (optional).</param>
        /// <returns>
        /// A collection of <see cref="Product"/> objects.
        /// </returns>
        /// <remarks>
        /// Example usage:
        /// 
        ///     var products = await _productService.GetAllProductsAsync(DateTime.Now.AddMonths(-1));
        /// </remarks>
        Task<IEnumerable<Product>> GetAllProductsAsync(DateTime? updatedAfter = null);

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// Example usage:
        /// 
        ///     var newProduct = new Product { Name = "New Product", Price = 99.99 };
        ///     await _productService.AddProductAsync(newProduct);
        /// </remarks>
        Task AddProductAsync(Product product);

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="product">The product with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// Example usage:
        /// 
        ///     var existingProduct = await _productService.GetProductByIdAsync(product.Id);
        ///     existingProduct.Name = "Updated Name";
        ///     await _productService.UpdateProductAsync(existingProduct);
        /// </remarks>
        Task UpdateProductAsync(Product product);
    }
}
