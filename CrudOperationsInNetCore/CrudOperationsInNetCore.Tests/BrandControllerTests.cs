 
 
using CrudOperationsInNetCore.Controllers;
using CrudOperationsInNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CrudOperationsInNetCore.Tests
{
    public class BrandControllerTests
    {
        private readonly Mock<BrandContext> _mockDbContext;
        private readonly BrandController _controller;

        public BrandControllerTests()
        {
            var options = new DbContextOptionsBuilder<BrandContext>()
                .UseInMemoryDatabase(databaseName: "BrandDb")
                .Options;

            _mockDbContext = new Mock<BrandContext>(options);
            _controller = new BrandController(_mockDbContext.Object);
        }

        [Fact]
        public async Task GetBrands_ReturnsOkResult_WhenBrandsExist()
        {
            // Arrange
            var mockBrands = new List<Brand>
            {
                new Brand { ID = 1, Name = "Brand1" },
                new Brand { ID = 2, Name = "Brand2" }
            };

            _mockDbContext.Object.Brands.AddRange(mockBrands);
            await _mockDbContext.Object.SaveChangesAsync();

            // Act
            var result = await _controller.GetBrands();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var brands = Assert.IsAssignableFrom<List<Brand>>(okResult.Value);
            Assert.Equal(2, brands.Count);
        }

        [Fact]
        public async Task GetBrands_ReturnsNotFound_WhenNoBrandsExist()
        {
            // Act
            var result = await _controller.GetBrands();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No brands found in the database.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetBrand_ReturnsOkResult_WhenBrandExists()
        {
            // Arrange
            var brand = new Brand { ID = 1, Name = "Brand1" };
            _mockDbContext.Object.Brands.Add(brand);
            await _mockDbContext.Object.SaveChangesAsync();

            // Act
            var result = await _controller.GetBrand(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBrand = Assert.IsType<Brand>(okResult.Value);
            Assert.Equal(1, returnedBrand.ID);
        }

        [Fact]
        public async Task GetBrand_ReturnsNotFound_WhenBrandDoesNotExist()
        {
            // Act
            var result = await _controller.GetBrand(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No brand found with ID: 999", notFoundResult.Value);
        }

        [Fact]
        public async Task PostBrand_ReturnsCreatedAtAction_WhenBrandIsCreated()
        {
            // Arrange
            var brand = new Brand { Name = "NewBrand" };

            // Act
            var result = await _controller.PostBrand(brand);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdBrand = Assert.IsType<Brand>(createdAtActionResult.Value);
            Assert.Equal("NewBrand", createdBrand.Name);
        }

        [Fact]
        public async Task PostBrand_ReturnsBadRequest_WhenBrandIsNull()
        {
            // Act
            var result = await _controller.PostBrand(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Brand cannot be null.", badRequestResult.Value);
        }

        [Fact]
        public async Task PutBrand_ReturnsOk_WhenBrandIsUpdated()
        {
            // Arrange
            var brand = new Brand { ID = 1, Name = "Brand1" };
            _mockDbContext.Object.Brands.Add(brand);
            await _mockDbContext.Object.SaveChangesAsync();

            brand.Name = "UpdatedBrand";

            // Act
            var result = await _controller.PutBrand(1, brand);

            // Assert
            var okResult = Assert.IsType<OkResult>(result.Result);
        }

        [Fact]
        public async Task PutBrand_ReturnsBadRequest_WhenBrandIDDoesNotMatch()
        {
            // Arrange
            var brand = new Brand { ID = 1, Name = "Brand1" };
            _mockDbContext.Object.Brands.Add(brand);
            await _mockDbContext.Object.SaveChangesAsync();

            brand.Name = "UpdatedBrand";

            // Act
            var result = await _controller.PutBrand(2, brand);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("The provided ID does not match the brand ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteBrand_ReturnsOk_WhenBrandIsDeleted()
        {
            // Arrange
            var brand = new Brand { ID = 1, Name = "Brand1" };
            _mockDbContext.Object.Brands.Add(brand);
            await _mockDbContext.Object.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteBrand(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("Brand with ID: 1 has been successfully deleted.", okResult.Value);
        }

        [Fact]
        public async Task DeleteBrand_ReturnsNotFound_WhenBrandDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteBrand(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No brand found with ID: 999", notFoundResult.Value);
        }
    }
}
