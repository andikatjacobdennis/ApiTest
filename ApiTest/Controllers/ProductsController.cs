using ApiTest.Contracts.Models;
using ApiTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiTest.Controllers
{
    /// <summary>
    /// Handles CRUD operations for products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="productService">The product service to interact with the product data.</param>
        public ProductsController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product to create.</param>
        /// <returns>
        /// A <see cref="CreatedAtActionResult"/> with the created product if successful (HTTP 201).
        /// A <see cref="BadRequestResult"/> if the model state is invalid (HTTP 400).
        /// A <see cref="ConflictResult"/> if the product already exists (HTTP 409).
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody, Required] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = await _productService.GetProductByIdAsync(product.Id);
            if (existingProduct != null)
                return Conflict("Product already exists.");

            product.Id = Guid.NewGuid();
            product.Created = DateTime.Now;
            product.LastUpdated = DateTime.Now;

            await _productService.AddProductAsync(product);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }


        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>
        /// A <see cref="OkObjectResult"/> containing the product if found (HTTP 200).
        /// A <see cref="NotFoundResult"/> if the product is not found (HTTP 404).
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        /// <summary>
        /// Retrieves a list of products with optional pagination and filtering.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination (default is 1).</param>
        /// <param name="pageSize">The number of products per page (default is 10).</param>
        /// <param name="updatedAfter">Filter products that were updated after this date.</param>
        /// <returns>A <see cref="OkObjectResult"/> containing the list of products (HTTP 200).</returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? updatedAfter = null)
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="updatedProduct">The updated product data.</param>
        /// <returns>
        /// A <see cref="OkObjectResult"/> with the updated product if successful (HTTP 200).
        /// A <see cref="BadRequestResult"/> if the model state is invalid (HTTP 400).
        /// A <see cref="NotFoundResult"/> if the product is not found (HTTP 404).
        /// </returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody, Required] Product updatedProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.LastUpdated = DateTime.Now;

            await _productService.UpdateProductAsync(product);

            return Ok(product);
        }
    }
}
