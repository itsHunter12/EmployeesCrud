using EmployeesCrud.IRepository;
using EmployeesCrud.Models;

namespace EmployeesCrud.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CityRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<City> GetAllCitiesByStateId(int stateId)
        {
            return _dbContext.City.Where(d => d.StateId == stateId).ToList();
        }
        
    }
}
