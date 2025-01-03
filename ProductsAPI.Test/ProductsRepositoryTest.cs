using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Models;
using ProductsAPI.Repository;

namespace ProductsAPI.Tests.Repository
{
    [TestClass]
    public class ProductRepositoryTests
    {
        private ProductContext _context;
        private ProductRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            
            _context = new ProductContext(options);

            var category = new Category { Name = "Default Category" };  // Id auto generated
            _context.Categories.Add(category);
            _context.SaveChanges(); 

            // Use the actual category.Id for products
            var product1 = new Product
            {
                Name = "Test Product 1",
                CategoryId = category.Id
            };
            var product2 = new Product
            {
                Name = "Test Product 2",
                CategoryId = category.Id
            };
            
            _context.Products.AddRange(product1, product2);
            _context.SaveChanges();

            _repository = new ProductRepository(_context);
        }


        [TestMethod]
        public async Task GetProductsAsync_ReturnsAllProducts()
        {
            // Act
            var products = await _repository.GetProductsAsync();

            // Assert
            Assert.IsNotNull(products, "Products list should not be null.");
            Assert.AreEqual(2, products.Count(), "Should return 2 products.");
        }

        [TestMethod]
        public async Task GetProductAsync_WhenProductExists_ReturnsProduct()
        {
            // Arrange
            var existingProductId = 1;

            // Act
            var product = await _repository.GetProductAsync(existingProductId);

            // Assert
            Assert.IsNotNull(product, "Product should not be null.");
            Assert.AreEqual(existingProductId, product.Id, "Returned product ID should match requested ID.");
        }

        [TestMethod]
        public async Task GetProductAsync_WhenProductDoesNotExist_ReturnsNull()
        {
            // Arrange
            var nonExistingProductId = 9999;

            // Act
            var product = await _repository.GetProductAsync(nonExistingProductId);

            // Assert
            Assert.IsNull(product, "Product should be null for non-existing ID.");
        }

        [TestMethod]
        public async Task AddProductAsync_WhenCategoryExists_AddsProduct()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "New Test Product",
                CategoryId = 1
            };

            // Act
            var createdProduct = await _repository.AddProductAsync(newProduct);
            var retrievedProduct = await _context.Products.FindAsync(createdProduct.Id);

            // Assert
            Assert.IsNotNull(createdProduct, "Created product should not be null.");
            Assert.IsNotNull(retrievedProduct, "Product should be found in the database.");
            Assert.AreEqual("New Test Product", retrievedProduct.Name, "Product name should match.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddProductAsync_WhenCategoryDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "Product with invalid category",
                CategoryId = 9999 // Non-existent Category
            };

            // Act
            // Expect ArgumentException to be thrown
            await _repository.AddProductAsync(newProduct);
        }

        [TestMethod]
        public async Task UpdateProductAsync_UpdatesExistingProduct()
        {
            // Arrange
            var existingProduct = await _context.Products.FindAsync(1);
            existingProduct.Name = "Updated Name";

            // Act
            var updatedProduct = await _repository.UpdateProductAsync(existingProduct);
            var retrievedProduct = await _context.Products.FindAsync(1);

            // Assert
            Assert.IsNotNull(updatedProduct, "Updated product should not be null.");
            Assert.AreEqual("Updated Name", retrievedProduct.Name, "Product name should be updated in the database.");
        }

        [TestMethod]
        public async Task DeleteProductAsync_WhenProductExists_ReturnsTrueAndRemovesProduct()
        {
            // Arrange
            var existingProductId = 1;

            // Act
            var result = await _repository.DeleteProductAsync(existingProductId);
            var deletedProduct = await _context.Products.FindAsync(existingProductId);

            // Assert
            Assert.IsTrue(result, "Delete should return true for existing product.");
            Assert.IsNull(deletedProduct, "Deleted product should not be found in the database.");
        }

        [TestMethod]
        public async Task DeleteProductAsync_WhenProductDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var nonExistingProductId = 9999;

            // Act
            var result = await _repository.DeleteProductAsync(nonExistingProductId);

            // Assert
            Assert.IsFalse(result, "Delete should return false for non-existing product.");
        }
    }
}
