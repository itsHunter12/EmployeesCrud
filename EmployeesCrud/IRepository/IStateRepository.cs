using EmployeesCrud.Models;

namespace EmployeesCrud.IRepository
{
    public interface IStateRepository
    {
        IList<State> GetAllStatesByCountryId(int countryId);
    }
}
