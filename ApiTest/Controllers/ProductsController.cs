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
        /// <param name="productService">The product service used to interact with product data.</param>
        public ProductsController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product to create. Must include name, description, and price.</param>
        /// <returns>
        /// A <see cref="CreatedAtActionResult"/> with the created product if successful (HTTP 201).
        /// <list type="bullet">
        /// <item><description><see cref="Product"/>: The created product including the generated ID.</description></item>
        /// </list>
        /// <response code="201">Returns the newly created product.</response>
        /// <response code="400">If the model is invalid.</response>
        /// <response code="409">If a product with the same ID already exists.</response>
        /// </returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/products
        ///     {
        ///        "name": "Product A",
        ///        "description": "Description for Product A",
        ///        "price": 19.99
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(Product), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
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
        /// A <see cref="OkObjectResult"/> containing the product if found.
        /// <response code="200">Returns the product details.</response>
        /// <response code="404">If the product is not found.</response>
        /// </returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/products/{id}
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(404)]
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
        /// <param name="updatedAfter">Filter products that were updated after this date.</param>
        /// <returns>
        /// A list of products with pagination information.
        /// <response code="200">Returns the list of products.</response>
        /// </returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/products?updatedAfter=2023-01-01
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        public async Task<IActionResult> GetProducts(
            [FromQuery] DateTime? updatedAfter = null)
        {
            var products = await _productService.GetAllProductsAsync(updatedAfter);
            return Ok(products);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="updatedProduct">The updated product data including name, description, and price.</param>
        /// <returns>
        /// A <see cref="OkObjectResult"/> with the updated product if successful.
        /// <response code="200">Returns the updated product.</response>
        /// <response code="400">If the model is invalid.</response>
        /// <response code="404">If the product is not found.</response>
        /// </returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PATCH /api/products/{id}
        ///     {
        ///        "name": "Updated Product A",
        ///        "description": "Updated description for Product A",
        ///        "price": 29.99
        ///     }
        /// </remarks>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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
