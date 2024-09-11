using ApiTest.Contracts.Models;
using ApiTest.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ApiTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] DateTime? updatedAfter = null)
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

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
