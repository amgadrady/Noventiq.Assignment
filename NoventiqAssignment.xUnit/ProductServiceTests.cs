using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NoventiqAssignment.DB.Models;
using NoventiqAssignment.Repository;
using NoventiqAssignment.Services;
using NoventiqAssignment.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NoventiqAssignment.xUnit
{
    public class ProductServiceTests
    {
        private readonly Mock<IMultiLanguageService> mockMultiLanguageService;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly ProductService productService;
        private readonly Mock<IGenericRepository<Product>> mockProductRepository;

        public ProductServiceTests()
        {
            mockMultiLanguageService = new Mock<IMultiLanguageService>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockProductRepository = new Mock<IGenericRepository<Product>>();
            mockUnitOfWork.Setup(u => u.Repository<Product>()).Returns(mockProductRepository.Object);

            productService = new ProductService(
                mockMultiLanguageService.Object,
                mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetProductByIdAsync_ProductExistsWithTranslation_ReturnsProductDto()
        {
            // Arrange
            var productId = 1;
            var languageCode = "en";
            var testProduct = new Product
            {
                Id = productId,
                Price = 19.99m,
                Translations = new List<ProductTranslation>
                {
                    new ProductTranslation
                    {
                        LanguageCode = languageCode,
                        Name = "Test Product",
                        Description = "Test Description"
                    }
                }
            };

            mockMultiLanguageService.Setup(m => m.GetCurrentLanguage()).Returns(languageCode);
            mockProductRepository.Setup(r => r.GetData(
     It.IsAny<List<Expression<Func<Product, bool>>>>(),
     It.IsAny<List<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>>(),
     false)) 
     .ReturnsAsync(new List<Product> { testProduct });

            // Act
            var result = await productService.GetProductByIdAsync(productId);

            // Assert
            
            Assert.NotNull(result.Data);
            Assert.Equal(productId, result.Data.Id);
            Assert.Equal("Test Product", result.Data.Name);
            Assert.Equal("Test Description", result.Data.Description);
            Assert.Equal(19.99m, result.Data.Price);
        }

        [Fact]
        public async Task GetProductByIdAsync_ProductDoesNotExist_ReturnsErrorResponse()
        {
            // Arrange
            var productId = 999;
            var languageCode = "en";

            mockMultiLanguageService.Setup(m => m.GetCurrentLanguage()).Returns(languageCode);
            mockProductRepository.Setup(r => r.GetData(
                It.IsAny<List<Expression<Func<Product, bool>>>>(),
                It.IsAny<List<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>>(),
                false))
         .ReturnsAsync(new List<Product>());

            // Act
            var result = await productService.GetProductByIdAsync(productId);

            // Assert
            Assert.Equal("Product not found", result.Message);
        }
        [Fact]
        public async Task GetProductByIdAsync_FiltersByCorrectLanguageCode()
        {
            // Arrange
            var productId = 1;
            var languageCode = "en";
            var testProduct = new Product
            {
                Id = productId,
                Price = 19.99m,
                Translations = new List<ProductTranslation>
                {
                    new ProductTranslation { LanguageCode = "en", Name = "English Name" ,Description="English description" },
                    new ProductTranslation { LanguageCode = "es", Name = "Spanish Name" ,Description=" Spanish descripción"}
                }
            };

            mockMultiLanguageService.Setup(m => m.GetCurrentLanguage()).Returns(languageCode);

            List<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>> capturedIncludes = null;

            mockProductRepository.Setup(r => r.GetData(
         It.IsAny<List<Expression<Func<Product, bool>>>>(),
         It.IsAny<List<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>>(),
         false))
                 .Callback<IList<Expression<Func<Product, bool>>>, List<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>, bool>(
         (filters, includes, enableTracking) => capturedIncludes = includes)
                     .ReturnsAsync(new List<Product> { testProduct });

            // Act
            await productService.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(capturedIncludes);
            Assert.Single(capturedIncludes);

          
        }

    }
}
