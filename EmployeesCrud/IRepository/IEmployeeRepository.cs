using EmployeesCrud.Models;
using EmployeesCrud.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EmployeesCrud.IRepository
{
    public interface IEmployeeRepository
    {
        IList<EmployeesViewModel> GetAllEmployees();

        EmployeeMaster Get(int id);

        bool Update(EmployeeMaster employee);
        bool CreateEmployee(EmployeeMaster employee);

        bool Delete(int id);
    }
}
