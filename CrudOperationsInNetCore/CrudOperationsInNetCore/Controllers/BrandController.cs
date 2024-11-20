using CrudOperationsInNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CrudOperationsInNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {

        private readonly BrandContext _dbContext;



        public BrandController(BrandContext dbContext)
        {
            _dbContext = dbContext;




        }




 

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            try
            {
                if (_dbContext.Brands == null)
                {
                    return NotFound("No brands found in the database.");
                }

                var brands = await _dbContext.Brands.ToListAsync();
                return Ok(brands);
            }
            catch (Exception ex)
            {
                // Log the exception if a logging mechanism is available
                // _logger.LogError(ex, "Error occurred while retrieving brands.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "An error occurred while processing your request.");
            }
        }





       

        [HttpGet("{ID}")]
        public async Task<ActionResult<Brand>> GetBrand(int ID)
        {
            try
            {
                if (_dbContext.Brands == null)
                {
                    return NotFound("The database context is not initialized.");
                }

                var brand = await _dbContext.Brands.FindAsync(ID);

                if (brand == null)
                {
                    return NotFound($"No brand found with ID: {ID}");
                }

                return Ok(brand);
            }
            catch (Exception ex)
            {
                // Log the exception if logging is available
                // _logger.LogError(ex, $"An error occurred while retrieving the brand with ID: {ID}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"An error occurred while processing your request: {ex.Message}");
            }
        }



         

        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            try
            {
                if (brand == null)
                {
                    return BadRequest("Brand cannot be null.");
                }

                // Add the new brand to the database
                _dbContext.Brands.Add(brand);
                await _dbContext.SaveChangesAsync();

                // Return a response indicating the brand was successfully created
                return CreatedAtAction(nameof(GetBrand), new { id = brand.ID }, brand);
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-specific issues, such as constraint violations
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"A database error occurred while adding the brand: {dbEx.Message}");
            }
            catch (ArgumentNullException argEx)
            {
                // Handle cases where a null object is unexpectedly passed
                return BadRequest($"Invalid input: {argEx.Message}");
            }
            catch (InvalidOperationException invalidOpEx)
            {
                // Handle invalid operations like adding an entity with duplicate keys
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"An invalid operation occurred: {invalidOpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle all other unforeseen exceptions
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"An unexpected error occurred: {ex.Message}");
            }
        }


        [HttpPut("{ID}")]
        public async Task<ActionResult<Brand>> PutBrand(int ID, Brand brand)
        {
            // Check if the provided ID matches the ID in the brand object
            if (ID != brand.ID)
            {
                return BadRequest("The provided ID does not match the brand ID.");
            }

            // Mark the entity as modified for update
            _dbContext.Entry(brand).State = EntityState.Modified;

            try
            {
                // Attempt to save the changes to the database
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbConcurrencyEx)
            {
                // Handle concurrency issues where data is being updated by someone else at the same time
                if (!BrandAvailable(ID))
                {
                    return NotFound($"Brand with ID {ID} not found.");
                }
                else
                {
                    // Log or rethrow the exception for further inspection
                    // _logger.LogError(dbConcurrencyEx, "Concurrency error occurred during brand update.");
                    throw;  // Rethrow the exception after logging if needed
                }
            }
            catch (DbUpdateException dbEx)
            {
                // Handle other database-specific exceptions such as constraint violations
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"A database error occurred while updating the brand: {dbEx.Message}");
            }
            catch (ArgumentNullException argEx)
            {
                // Handle cases where a null argument is provided
                return BadRequest($"Invalid argument provided: {argEx.Message}");
            }
            catch (InvalidOperationException invalidOpEx)
            {
                // Handle invalid operations like updating an entity in an incorrect state
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"An invalid operation occurred: {invalidOpEx.Message}");
            }
            catch (Exception ex)
            {
                // Catch all other exceptions and return a generic error message
                // Log the error if necessary
                // _logger.LogError(ex, "An unexpected error occurred while updating the brand.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"An unexpected error occurred: {ex.Message}");
            }

            // Return an OK status if the update is successful
            return Ok();
        }



        private bool BrandAvailable(int id)
        {
            return (_dbContext.Brands?.Any(x => x.ID == id)).GetValueOrDefault();



        }


        [HttpDelete("{ID}")]
        public async Task<ActionResult<Brand>> DeleteBrand(int ID)
        {
            try
            {
                // Check if the Brands DbSet is null
                if (_dbContext.Brands == null)
                {
                    return NotFound("The database context is not initialized.");
                }

                // Fetch the brand to delete
                var brand = await _dbContext.Brands.FirstOrDefaultAsync(x => x.ID == ID);

                // If the brand is not found
                if (brand == null)
                {
                    return NotFound($"No brand found with ID: {ID}");
                }

                // Remove the brand from the database
                _dbContext.Brands.Remove(brand);
                await _dbContext.SaveChangesAsync();

                // Return success response
                return Ok($"Brand with ID: {ID} has been successfully deleted.");
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-specific issues such as constraint violations
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"A database error occurred while deleting the brand: {dbEx.Message}");
            }
            catch (ArgumentNullException argEx)
            {
                // Handle cases where a null object is unexpectedly passed
                return BadRequest($"Invalid argument provided: {argEx.Message}");
            }
            catch (InvalidOperationException invalidOpEx)
            {
                // Handle invalid operations like trying to delete an entity in an invalid state
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"An invalid operation occurred: {invalidOpEx.Message}");
            }
            catch (Exception ex)
            {
                // Catch all other unforeseen exceptions
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"An unexpected error occurred: {ex.Message}");
            }
        }





    }


}
