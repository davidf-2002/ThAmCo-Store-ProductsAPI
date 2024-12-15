using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProductsAPI.Data;
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

    }
}