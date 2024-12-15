using Microsoft.Extensions.Logging;
using Moq;
using ProductsAPI.Models;
using ProductsAPI.Repository;
using ThAmCo.ProductsAPI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ThAmCo.ProductsAPI.Test
{
    [TestClass]
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockRepo.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task GetProducts_ReturnsOkObjectResult_WithAListOfProducts()
        {
            // Arrange
            var mockProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 1000},
                new Product { Id = 2, Name = "Phone", Price = 800}
            };

            _mockRepo.Setup(repo => repo.GetProductsAsync()).ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(List<Product>));
            var productsList = okResult.Value as List<Product>;
            Assert.AreEqual(2, productsList.Count);
        }

        [TestMethod]
        public async Task GetProduct_ReturnsOkObjectResult_WithProduct()
        {
            // Arrange
            var mockProduct = new Product { Id = 1, Name = "Laptop", Price = 1000 };
            _mockRepo.Setup(repo => repo.GetProductAsync(1)).ReturnsAsync(mockProduct);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(mockProduct, okResult.Value as Product);
        }

        [TestMethod]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetProductAsync(1)).ReturnsAsync(value: null);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }

        [TestMethod]
        public async Task PostProduct_ReturnsCreatedAtActionResult_WhenValidProduct()
        {
            // Arrange
            var newProduct = new Product { Name = "Tablet", Price = 300 };
            _mockRepo.Setup(repo => repo.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(new Product { Id = 3, Name = "Tablet", Price = 300 });

            // Act
            var result = await _controller.PostProduct(newProduct);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetProduct", createdResult.ActionName);
            Assert.IsInstanceOfType(createdResult.Value, typeof(Product));
        }

        [TestMethod]
        public async Task PostProduct_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var newProduct = new Product { Name = "", Price = 300 }; // Invalid state
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.PostProduct(newProduct);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.IsTrue(_controller.ModelState.ErrorCount > 0);
        }

        [TestMethod]
        public async Task PutProduct_ReturnsOkObjectResult_WhenProductIsUpdatedSuccessfully()
        {
            // Arrange
            var existingProduct = new Product { Id = 1, Name = "Laptop", Price = 1000 };
            _mockRepo.Setup(repo => repo.GetProductAsync(1)).ReturnsAsync(existingProduct);
            _mockRepo.Setup(repo => repo.UpdateProductAsync(It.IsAny<Product>())).ReturnsAsync(existingProduct);

            // Act
            var result = await _controller.PutProduct(1, existingProduct);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(existingProduct, okResult.Value as Product);
        }

        [TestMethod]
        public async Task PutProduct_ReturnsBadRequest_WhenProductIdMismatch()
        {
            // Arrange
            var productToUpdate = new Product { Id = 2, Name = "Smartphone", Price = 900 };

            // Act
            var result = await _controller.PutProduct(1, productToUpdate);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            var badRequestResult = result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);
        }

        [TestMethod]
        public async Task PutProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var nonExistingProduct = new Product { Id = 3, Name = "Tablet", Price = 300 };
            _mockRepo.Setup(repo => repo.GetProductAsync(3)).ReturnsAsync(value: null);

            // Act
            var result = await _controller.PutProduct(3, nonExistingProduct);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
        }

        [TestMethod]
        public async Task DeleteProduct_ReturnsOkResult_WhenProductDeletedSuccessfully()
        {
            // Arrange
            var existingProduct = new Product { Id = 1, Name = "Laptop", Price = 1000 };
            _mockRepo.Setup(repo => repo.GetProductAsync(1)).ReturnsAsync(existingProduct);
            _mockRepo.Setup(repo => repo.DeleteProductAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Product with ID 1 has been deleted.", okResult.Value);
        }

        [TestMethod]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetProductAsync(1)).ReturnsAsync(value: null);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
        }

        [TestMethod]
        public async Task DeleteProduct_ReturnsBadRequest_WhenDeleteFails()
        {
            // Arrange
            var existingProduct = new Product { Id = 1, Name = "Laptop", Price = 1000 };
            _mockRepo.Setup(repo => repo.GetProductAsync(1)).ReturnsAsync(existingProduct);
            _mockRepo.Setup(repo => repo.DeleteProductAsync(1)).ReturnsAsync(false); // Simulate delete operation failure

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
        }
        
    }
}