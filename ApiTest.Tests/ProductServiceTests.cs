using ApiTest.Contracts.Models;
using ApiTest.Entity.Data;
using ApiTest.Services;
using Microsoft.EntityFrameworkCore;

namespace ApiTest.Tests
{
    [TestClass]
    public class ProductServiceTests
    {
        private DbContextOptions<ProductDbContext> _dbContextOptions;
        private ProductService _service;

        public ProductServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            var context = new ProductDbContext(_dbContextOptions);
            _service = new ProductService(context);
        }

        [TestInitialize]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            var context = new ProductDbContext(_dbContextOptions);
            _service = new ProductService(context);
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ReturnsNull_WhenProductDoesNotExist()
        {
            // Act
            var result = await _service.GetProductByIdAsync(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetProductByIdAsync_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Name = "Test Product" };
            using (var context = new ProductDbContext(_dbContextOptions))
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _service.GetProductByIdAsync(product.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(product.Id, result.Id);
        }

        [TestMethod]
        public async Task GetAllProductsAsync_ReturnsProducts_WhenProductsExist()
        {
            // Arrange
            var product1 = new Product { Id = Guid.NewGuid(), Name = "Product 1" };
            var product2 = new Product { Id = Guid.NewGuid(), Name = "Product 2" };
            using (var context = new ProductDbContext(_dbContextOptions))
            {
                context.Products.AddRange(product1, product2);
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            Assert.IsTrue(result.Any(p => p.Id == product1.Id));
            Assert.IsTrue(result.Any(p => p.Id == product2.Id));
        }

        [TestMethod]
        public async Task AddProductAsync_AddsProductSuccessfully()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Name = "New Product" };

            // Act
            await _service.AddProductAsync(product);

            // Assert
            using var context = new ProductDbContext(_dbContextOptions);
            var result = await context.Products.FindAsync(product.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(product.Name, result.Name);
        }

        [TestMethod]
        public async Task UpdateProductAsync_UpdatesProductSuccessfully()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Name = "Old Name" };
            using (var context = new ProductDbContext(_dbContextOptions))
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
            }

            var updatedProduct = new Product { Id = product.Id, Name = "New Name" };

            // Act
            await _service.UpdateProductAsync(updatedProduct);

            // Assert
            using (var context = new ProductDbContext(_dbContextOptions))
            {
                var result = await context.Products.FindAsync(product.Id);
                Assert.IsNotNull(result);
                Assert.AreEqual("New Name", result.Name);
            }
        }
    }
}
