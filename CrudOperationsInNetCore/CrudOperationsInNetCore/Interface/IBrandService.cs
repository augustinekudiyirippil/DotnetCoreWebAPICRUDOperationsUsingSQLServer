using CrudOperationsInNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrudOperationsInNetCore.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<Brand>> GetBrandsAsync();
        Task<Brand?> GetBrandAsync(int id);
        Task<Brand> AddBrandAsync(Brand brand);
        Task<bool> UpdateBrandAsync(int id, Brand brand);
        Task<bool> DeleteBrandAsync(int id);
    }
}
