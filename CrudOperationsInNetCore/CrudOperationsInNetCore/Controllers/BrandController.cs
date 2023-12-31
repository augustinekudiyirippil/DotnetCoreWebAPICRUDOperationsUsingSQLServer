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



        private BrandController(BrandContext dbContext)
        {
            _dbContext = dbContext;




        }




        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {

            if (_dbContext.Brands == null)
            {

                return NotFound();
            }


            return await _dbContext.Brands.ToListAsync();

        }






        [HttpGet("{ID}")]
        public async Task<ActionResult<Brand>> GetBrand(int ID)
        {

            if (_dbContext.Brands == null)
            {

                return NotFound();
            }


            var brand = await _dbContext.Brands.FindAsync(ID);

            if (brand == null)
            {
                return NotFound();
            }

            return brand;

        }



        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {


            _dbContext.Brands.Add(brand);


            await _dbContext.SaveChangesAsync();


            return CreatedAtAction(nameof(GetBrand), new { id = brand.ID }, brand);



        }


        [HttpPut]
        public async Task<ActionResult<Brand>> PutBrand(int ID, Brand brand)
        {


            if (ID != brand.ID)
            {

                return BadRequest();
            }

            _dbContext.Entry(brand).State = EntityState.Modified;


            try
            {
                await _dbContext.SaveChangesAsync();


            }
            catch (DbUpdateConcurrencyException)
            {

                if (!BrandAvailable(ID))
                {

                    return NotFound();
                }
                else
                {
                    throw;
                }

            }

            return Ok();

        }



        private bool BrandAvailable(int id)
        {
            return (_dbContext.Brands?.Any(x => x.ID == id)).GetValueOrDefault();



        }



        [HttpDelete("{ID}")]
        public async Task<ActionResult<Brand>> DeleteBrand(int ID)
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }



            var brand = _dbContext.Brands.FirstOrDefault(x => x.ID == ID);

            if (brand == null)
            {
                return NotFound();

            }
            else

            {
                _dbContext.Brands.Remove(brand);
                await _dbContext.SaveChangesAsync();

                return Ok();


            }
        }




    }
     

    }
