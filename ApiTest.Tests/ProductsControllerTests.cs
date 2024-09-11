using ApiTest.Contracts.Models;
using ApiTest.Controllers;
using ApiTest.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiTest.Tests
{
    [TestClass]
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<IProductService> _mockProductService;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
        }

        [TestMethod]
        public async Task CreateProduct_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("key", "error");

            // Act
            var result = await _controller.CreateProduct(new Product());

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task CreateProduct_ReturnsConflict_WhenProductAlreadyExists()
        {
            // Arrange
            var existingProduct = new Product { Id = Guid.NewGuid() };
            var newProduct = new Product { Id = existingProduct.Id };
            _mockProductService.Setup(s => s.GetProductByIdAsync(existingProduct.Id)).ReturnsAsync(existingProduct);

            // Act
            var result = await _controller.CreateProduct(newProduct);

            // Assert
            var conflictResult = result as ConflictObjectResult;
            Assert.IsNotNull(conflictResult);
            Assert.AreEqual(409, conflictResult.StatusCode);
            Assert.AreEqual("Product already exists.", conflictResult.Value);
        }

        [TestMethod]
        public async Task CreateProduct_ReturnsCreatedAtAction_WhenProductIsSuccessfullyCreated()
        {
            // Arrange
            var newProduct = new Product { Name = "New Product" };
            _mockProductService.Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);
            _mockProductService.Setup(s => s.AddProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateProduct(newProduct);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetProduct", createdResult.ActionName);
            Assert.IsInstanceOfType(createdResult.Value, typeof(Product));
        }

        [TestMethod]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mockProductService.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task GetProduct_ReturnsOk_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId };
            _mockProductService.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(Product));
        }

        [TestMethod]
        public async Task UpdateProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var updatedProduct = new Product { Id = productId };
            _mockProductService.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.UpdateProduct(productId, updatedProduct);

            // Assert
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task UpdateProduct_ReturnsOk_WhenProductIsSuccessfullyUpdated()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product { Id = productId };
            var updatedProduct = new Product { Id = productId, Name = "Updated Product" };
            _mockProductService.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync(existingProduct);
            _mockProductService.Setup(s => s.UpdateProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProduct(productId, updatedProduct);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(Product));
        }
    }
}
