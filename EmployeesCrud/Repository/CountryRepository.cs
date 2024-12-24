using EmployeesCrud.IRepository;
using EmployeesCrud.Models;

namespace EmployeesCrud.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CountryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<Country> GetAllCountries()
        {
            return _dbContext.Country.ToList();
        }
    }
}
