namespace CrudOperationsInNetCore.Models
{
    public class Brand
    {

        public int ID { get; set; }


         
        public string? Name { get; set; }

        public string? Category { get; set; }

        public string? IsActive { get; set; }

        public static implicit operator Brand(List<Brand> v)
        {
            throw new NotImplementedException();
        }
    }
}
