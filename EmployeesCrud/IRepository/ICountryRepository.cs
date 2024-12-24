using EmployeesCrud.Models;

namespace EmployeesCrud.IRepository
{
    public interface ICountryRepository
    {
        IList<Country> GetAllCountries();
    }
}
