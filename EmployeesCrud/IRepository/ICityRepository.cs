using EmployeesCrud.Models;

namespace EmployeesCrud.IRepository
{
    public interface ICityRepository
    {
        IList<City> GetAllCitiesByStateId(int stateId);
    }
}
