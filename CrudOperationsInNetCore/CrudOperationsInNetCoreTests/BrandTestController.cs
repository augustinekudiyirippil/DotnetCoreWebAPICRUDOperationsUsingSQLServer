 


using CrudOperationsInNetCore.Controllers;
using CrudOperationsInNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CrudOperationsInNetCore.Tests
{
    public class BrandTestController
    {
        private   Mock<BrandContext> _mockDbContext;
        private   BrandController _controller;

        public BrandTestController()
        {
            var options = new DbContextOptionsBuilder<BrandContext>()
                .UseInMemoryDatabase(databaseName: "BrandDb")
                .Options;

            _mockDbContext = new Mock<BrandContext>(options);
            _controller = new BrandController(_mockDbContext.Object);


        }

        [Fact]
        public async Task GetBrands_ReturnsOkResultWithBrands()
        {
            // Arrange: Use an in-memory database for testing
            var options = new DbContextOptionsBuilder<BrandContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_GetBrands")
                .Options;

            // Seed the in-memory database with test data
            using (var context = new BrandContext(options))
            {
                context.Brands.AddRange(new List<Brand>
            {
                new Brand { ID = 1, Name = "Ford" },
                new Brand { ID = 2, Name = "Honda" }
            });
                await context.SaveChangesAsync();
            }

            // Use a fresh context instance to mimic real controller usage
            using (var context = new BrandContext(options))
            {
                var controller = new BrandController(context);

                // Act: Call the method
                var result = await controller.GetBrands();

                // Assert: Verify the result
                var okResult = Assert.IsType<OkObjectResult>(result.Result); // Ensure result is OkObjectResult
                var brands = Assert.IsAssignableFrom<List<Brand>>(okResult.Value); // Ensure result value is a list of Brand objects
                Assert.Equal(2, brands.Count); // Verify the number of brands returned
                Assert.Contains(brands, b => b.Name == "Ford"); // Verify the content
                Assert.Contains(brands, b => b.Name == "Honda");
            }
        }






        [Fact]
        public async Task GetBrand_ReturnsOkResult_WhenBrandExists()
        {
            // Arrange: Set up an in-memory database and seed it with test data
            var options = new DbContextOptionsBuilder<BrandContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_GetBrand")
                .Options;

            using (var context = new BrandContext(options))
            {
                context.Brands.AddRange(new List<Brand>
            {
                new Brand { ID = 1, Name = "Ford" },
                new Brand { ID = 2, Name = "Honda" }
            });
                    await context.SaveChangesAsync();
            }

            // Act: Use the same context to create the controller and call the method
            using (var context = new BrandContext(options))
            {
                var controller = new BrandController(context);

                // Act: Call GetBrand with an existing ID
                var result = await controller.GetBrand(1); // ID 1 corresponds to Ford

                // Assert: Verify that the result is an OkObjectResult
                var okResult = Assert.IsType<OkObjectResult>(result.Result); // Ensure result is OkObjectResult
                var brand = Assert.IsAssignableFrom<Brand>(okResult.Value); // Ensure the result value is of type Brand
                Assert.Equal(1, brand.ID); // Check that the ID is the same as the requested one
                Assert.Equal("Ford", brand.Name); // Check that the name is correct
            }
        }





        
        [Fact]
        public async Task PostBrand_ReturnsCreatedAtAction_WhenBrandIsAdded()
        {
 
        var options = new DbContextOptionsBuilder<BrandContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase_PostBrand")
            .Options;

         

            // Initialize the DbContext with in-memory database
            using (var _mockDbContext = new BrandContext(options))
            {
                // Create the controller with the in-memory DbContext
                var _controller = new BrandController(_mockDbContext);

                // Arrange: Create a new brand object to be added
                var brand = new Brand { ID = 3, Name = "Tesla" };

                // Act: Call the PostBrand method
                var result = await _controller.PostBrand(brand);

                // Assert: Verify that the result is a CreatedAtAction result
                var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);

                // Assert: Verify that the returned brand matches the created brand
                var returnedBrand = Assert.IsAssignableFrom<Brand>(createdResult.Value);
                Assert.Equal(brand.ID, returnedBrand.ID);
                Assert.Equal(brand.Name, returnedBrand.Name);

                // Assert: Verify that the CreatedAtAction contains the correct route (GetBrand) and ID
                Assert.Equal(nameof(BrandController.GetBrand), createdResult.ActionName);
                Assert.Equal(brand.ID, createdResult.RouteValues["id"]);

                // Assert: Verify the brand is actually added to the database
                var brandInDb = await _mockDbContext.Brands.FindAsync(brand.ID);
                Assert.NotNull(brandInDb);
                Assert.Equal(brand.Name, brandInDb.Name);
            }
        }





        [Fact]
        public async Task PostBrand_ReturnsBadRequest_WhenBrandIsNull()
        {
            var options = new DbContextOptionsBuilder<BrandContext>()
          .UseInMemoryDatabase(databaseName: "TestDatabase_PostBrand")
          .Options;

            // Act: Call PostBrand with a null brand
            var result = await _controller.PostBrand(null);

            // Assert: Verify that the result is a BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Brand cannot be null.", badRequestResult.Value);
        }


 


    }
}

