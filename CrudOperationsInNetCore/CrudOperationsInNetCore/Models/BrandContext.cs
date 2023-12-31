using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CrudOperationsInNetCore.Models
{
    public class BrandContext : DbContext
    {


        public BrandContext(DbContextOptions<BrandContext> options) : base(options)
        {



        }

        public DbSet<Brand> Brands { get; set; }





    }
}
