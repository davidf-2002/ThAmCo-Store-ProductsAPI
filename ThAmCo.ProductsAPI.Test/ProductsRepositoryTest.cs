using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProductsAPI.Data;
using ProductsAPI.Models;
using ProductsAPI.Repository;

namespace ThAmCo.ProductsAPI.Test
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private Mock<ProductContext> _mockContext;
        private Mock<DbSet<Product>> _mockSet;
        private ProductRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "T-shirt", Description = "Cotton white", Price = 15.99m, StockStatus = "In Stock" },
                new Product { Id = 2, Name = "Running Shoes", Description = "Best for marathons", Price = 50.00m, StockStatus = "In Stock" }
            }.AsQueryable();

            _mockSet = new Mock<DbSet<Product>>();
            _mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            _mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            _mockContext = new Mock<ProductContext>();
            _mockContext.Setup(c => c.Products).Returns(_mockSet.Object);

            _repository = new ProductRepository(_mockContext.Object);
        }

        [TestMethod]
        public async Task GetProductsAsync_ReturnsAllProducts()
        {
            // Act
            var result = await _repository.GetProductsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("T-shirt", result.First().Name);
        }
    }
}