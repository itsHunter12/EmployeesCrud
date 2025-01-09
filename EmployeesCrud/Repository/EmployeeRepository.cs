using System.Linq.Expressions;
using AutoMapper;
using EmployeesCrud.IRepository;
using EmployeesCrud.Models;
using EmployeesCrud.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EmployeesCrud.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public EmployeeRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool CreateEmployee(EmployeeMaster employee)
        {
            try
            {
                _dbContext.EmployeeMaster.Add(employee);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            var existingEmployee = _dbContext.EmployeeMaster.Find(id);

            if (existingEmployee == null)
            {
                return false;
            }

            existingEmployee.DeletedDate = DateTime.Now;
            existingEmployee.IsDeleted = true;
            existingEmployee.UpdatedDate = DateTime.Now;

            _dbContext.SaveChanges();
            return true;
        }
        public EmployeeMaster Get(int id)
        {
            return _dbContext.EmployeeMaster
                    .Include(e => e.City)
                    .Include(e => e.Country)
                    .Include(e => e.State)
                    .FirstOrDefault(m => m.Row_Id == id);
        }
        public bool Update(EmployeeMaster employee)
        {
            var existingEmployee = _dbContext.EmployeeMaster.Find(employee.Row_Id);

            if (existingEmployee == null)
            {
                return false;
            }

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.CountryId = employee.CountryId;
            existingEmployee.StateId = employee.StateId;
            existingEmployee.CityId = employee.CityId;
            existingEmployee.EmailAddress = employee.EmailAddress;
            existingEmployee.MobileNumber = employee.MobileNumber;
            existingEmployee.PanNumber = employee.PanNumber;
            existingEmployee.PassportNumber = employee.PassportNumber;
            if (!string.IsNullOrEmpty(employee.ProfileImage))
            {
                existingEmployee.ProfileImage = employee.ProfileImage;
            }
            existingEmployee.Gender = employee.Gender;
            existingEmployee.IsActive = employee.IsActive;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.DateOfJoinee = employee.DateOfJoinee;

            if (employee.IsDeleted && !existingEmployee.IsDeleted)
            {
                existingEmployee.IsDeleted = employee.IsDeleted;
                existingEmployee.DeletedDate = DateTime.Now;
            }
            else if (!employee.IsDeleted)
            {
                existingEmployee.DeletedDate = null;
            }
            existingEmployee.UpdatedDate = DateTime.Now;

            _dbContext.SaveChanges();
            return true;
        }

        IList<EmployeesViewModel> IEmployeeRepository.GetAllEmployees()
        {
            var allemployees = _dbContext.EmployeeMaster
                .Where(e => !e.IsDeleted)
                .Include(e => e.City)
                .Include(e => e.Country)
                .Include(e => e.State).ToList();
            var employeesViewModels = _mapper.Map<List<EmployeesViewModel>>(allemployees);

            return employeesViewModels;
        }
    }
}
