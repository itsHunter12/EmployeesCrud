using EmployeesCrud.IRepository;
using EmployeesCrud.Models;

namespace EmployeesCrud.Repository
{
    public class StateRepository : IStateRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public StateRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<State> GetAllStatesByCountryId(int countryId)
        {
            return _dbContext.State.Where(d => d.CountryId == countryId).ToList();
        }
    }
}
