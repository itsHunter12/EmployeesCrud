using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeesCrud.Models;
using AutoMapper;
using EmployeesCrud.ViewModels;
using EmployeesCrud.IRepository;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;

namespace EmployeesCrud.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public EmployeesController(
        IEmployeeRepository employeeRepository,
        ICountryRepository countryRepository,
        IStateRepository stateRepository,
        ICityRepository cityRepository,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        // GET: Employees
        public IActionResult Index()
        {
            try
            {
                // Fetch all employees from the repository
                var employees = _employeeRepository.GetAllEmployees();
                return View(employees);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                ModelState.AddModelError(string.Empty, "An error occurred while fetching employees.");
                return View("Error"); // Redirect to an error view
            }
        }

        // GET: Employees/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                // Validate that the ID is provided
                if (id == 0)
                {
                    return NotFound(); // Return 404 if ID is null
                }

                // Fetch employee details with related entities (City, Country, State)
                var employeeMaster = _employeeRepository.Get(id);

                // Check if the employee record exists
                if (employeeMaster == null)
                {
                    return NotFound(); // Return 404 if employee is not found
                }

                return View(employeeMaster); // Return the details view
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                ModelState.AddModelError(string.Empty, "An error occurred while fetching employee details.");
                return View("Error"); // Redirect to an error view
            }
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            try
            {
                // Fetch list of countries for dropdown
                var countries = _countryRepository.GetAllCountries().Select(c => new SelectListItem
                {
                    Value = c.Row_Id.ToString(),
                    Text = c.CountryName
                }).ToList();

                // Populate ViewData with dropdown values
                ViewData["CountryId"] = countries;
                ViewData["StateId"] = new List<SelectListItem>(); // Empty list for State
                ViewData["CityId"] = new List<SelectListItem>();  // Empty list for City

                return View(); // Return the Create view
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                ModelState.AddModelError(string.Empty, "An error occurred while preparing the create form.");
                return View(); // Redirect to an error view
            }
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeMaster employeeMaster)
        {
            try
            {
                var existingList = _employeeRepository.GetAllEmployees();
                // Validate if PAN Number already exists
                if (existingList.Any(e => e.PanNumber == employeeMaster.PanNumber))
                {
                    ModelState.AddModelError("PanNumber", "PAN Number already exists.");
                }

                // Validate if Passport Number already exists
                if (existingList.Any(e => e.PassportNumber == employeeMaster.PassportNumber))
                {
                    ModelState.AddModelError("PassportNumber", "Passport Number already exists.");
                }

                // Check if the email address already exists
                if (existingList.Any(e => e.EmailAddress == employeeMaster.EmailAddress))
                {
                    ModelState.AddModelError("EmailAddress", "Email Address already exists.");
                }

                // Check if the mobile number already exists
                if (existingList.Any(e => e.MobileNumber == employeeMaster.MobileNumber))
                {
                    ModelState.AddModelError("MobileNumber", "Mobile Number already exists.");
                }

                // Check if email is valid
                if (string.IsNullOrWhiteSpace(employeeMaster.EmailAddress))
                {
                    ModelState.AddModelError("EmailAddress", "Email address is required.");
                }
                else
                {
                    var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                    if (!emailRegex.IsMatch(employeeMaster.EmailAddress))
                    {
                        ModelState.AddModelError("EmailAddress", "Please enter a valid email address.");
                    }
                }

                // Check if mobile number is valid
                if (string.IsNullOrWhiteSpace(employeeMaster.MobileNumber))
                {
                    ModelState.AddModelError("MobileNumber", "Mobile number is required.");
                }
                else if (!employeeMaster.MobileNumber.All(char.IsDigit))
                {
                    ModelState.AddModelError("MobileNumber", "Mobile number must contain only digits.");
                }
                else if (employeeMaster.MobileNumber.Length != 10)
                {
                    ModelState.AddModelError("MobileNumber", "Mobile number must be exactly 10 digits.");
                }

                // Handle file upload if provided
                if (employeeMaster.UploadedFile != null && employeeMaster.UploadedFile.Length > 0)
                {
                    // Define the path where the file will be saved
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads\\Employees");
                    Directory.CreateDirectory(uploadPath); // Ensure directory exists

                    string fileExtension = Path.GetExtension(employeeMaster.UploadedFile.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                    // Check file extension
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("UploadedFile", "Only JPG and PNG image formats are allowed.");
                        GetDrodown(employeeMaster); // Ensure dropdowns are repopulated
                        return View(employeeMaster); // Return the view with the validation error
                    }

                    const long maxFileSize = 200 * 1024; // 200 kb in bytes

                    if (employeeMaster.UploadedFile.Length > maxFileSize)
                    {
                        ModelState.AddModelError("UploadedFile", "The file size must not exceed 200 KB.");
                        GetDrodown(employeeMaster);
                        return View(employeeMaster); // Return the view with the validation error
                    }

                    string filePath = Path.Combine(uploadPath, Path.GetFileName(employeeMaster.UploadedFile.FileName));

                    if (System.IO.File.Exists(filePath))
                    {
                        ModelState.AddModelError("UploadedFile", "An image with the same name already exists. Please rename the file or choose a different image.");
                        GetDrodown(employeeMaster); // Ensure dropdowns are repopulated
                        return View(employeeMaster); // Return view with the error
                    }

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        employeeMaster.UploadedFile.CopyTo(fileStream);
                    }
                    employeeMaster.ProfileImage = employeeMaster.UploadedFile.FileName;
                }
                if (ModelState.IsValid)
                {
                    employeeMaster.CreatedDate = DateTime.Now;
                    employeeMaster.IsDeleted = false;

                    bool isCreated = _employeeRepository.CreateEmployee(employeeMaster);
                    if (isCreated)
                    {
                        // Set a success message in TempData
                        TempData["SuccessMessage"] = "Record created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
               
                GetDrodown(employeeMaster);
                return View(employeeMaster);
            }
            catch (Exception)
            {
                // Log the exception for debugging purposes
                ModelState.AddModelError(string.Empty, "An error occurred while creating the employee record.");
                return View(); // Redirect to an error view
            }

        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                // Check if the provided id is null
                if (id == 0)
                {
                    // Return a "Not Found" response if no id is passed
                    return NotFound();
                }

                // Retrieve the employee record from the database based on the id
                var employeeMaster = _employeeRepository.Get(id);

                // If the employee record is not found, return a "Not Found" response
                if (employeeMaster == null)
                {
                    return NotFound();
                }

                // Populate any dropdowns or additional data needed for the view
                GetDrodown(employeeMaster);

                // Return the view with the employeeMaster object
                return View(employeeMaster);
            }
            catch (Exception ex)
            {
                // Return a generic error view or a bad request status if an exception occurs
                return StatusCode(500, "Internal server error");
            }
        }       

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EmployeeMaster employeeMaster)
        {
            if (id != employeeMaster.Row_Id)
            {
                return NotFound();
            }

            var existingList= _employeeRepository.GetAllEmployees();
            // Exclude the current record while checking for uniqueness
            if (existingList.Any(e => e.PanNumber == employeeMaster.PanNumber && e.Row_Id != id))
            {
                ModelState.AddModelError("PanNumber", "PAN Number already exists.");
            }

            // Validate if Passport Number already exists
            if (existingList.Any(e => e.PassportNumber == employeeMaster.PassportNumber && e.Row_Id != id))
            {
                ModelState.AddModelError("PassportNumber", "Passport Number already exists.");
            }

            // Validate if Email already exists
            if (existingList.Any(e => e.EmailAddress == employeeMaster.EmailAddress && e.Row_Id != id))
            {
                ModelState.AddModelError("EmailAddress", "Email Address already exists.");
            }

            // Validate if Mobile Number already exists
            if (existingList.Any(e => e.MobileNumber == employeeMaster.MobileNumber && e.Row_Id != id))
            {
                ModelState.AddModelError("MobileNumber", "Mobile Number already exists.");
            }

            // Check if email is valid
            if (string.IsNullOrWhiteSpace(employeeMaster.EmailAddress))
            {
                ModelState.AddModelError("EmailAddress", "Email address is required.");
            }
            else
            {
                var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
                if (!emailRegex.IsMatch(employeeMaster.EmailAddress))
                {
                    ModelState.AddModelError("EmailAddress", "Please enter a valid email address.");
                }
            }

            // Check if mobile number is valid
            if (string.IsNullOrWhiteSpace(employeeMaster.MobileNumber))
            {
                ModelState.AddModelError("MobileNumber", "Mobile number is required.");
            }
            else if (!employeeMaster.MobileNumber.All(char.IsDigit))
            {
                ModelState.AddModelError("MobileNumber", "Mobile number must contain only digits.");
            }

            var existingEmployee = existingList.FirstOrDefault(e => e.Row_Id == employeeMaster.Row_Id);

            if (employeeMaster.UploadedFile != null && employeeMaster.UploadedFile.Length > 0)
            {
                // Define the path where the file will be saved
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads\\Employees");
                Directory.CreateDirectory(uploadPath); // Ensure directory exists

                string fileExtension = Path.GetExtension(employeeMaster.UploadedFile.FileName).ToLower();
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

                // Check file extension
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("UploadedFile", "Only JPG and PNG image formats are allowed.");
                    GetDrodown(employeeMaster); // Ensure dropdowns are repopulated
                    return View(employeeMaster); // Return the view with the validation error
                }

                const long maxFileSize = 200 * 1024; // 200 kb in bytes

                if (employeeMaster.UploadedFile.Length > maxFileSize)
                {
                    ModelState.AddModelError("UploadedFile", "The file size must not exceed 200 KB.");
                    GetDrodown(employeeMaster);
                    return View(employeeMaster); // Return the view with the validation error
                }

                // Check if the employee already has a profile image in the database
                if (existingEmployee != null && !string.IsNullOrEmpty(existingEmployee.ProfileImage))
                {
                    string existingFilePath = Path.Combine(uploadPath, existingEmployee.ProfileImage);

                    // Delete the existing file if it exists
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }
                }

                // Generate a unique filename using GUID
                string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                string filePath = Path.Combine(uploadPath, uniqueFileName);              

                // Save the new file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    employeeMaster.UploadedFile.CopyTo(fileStream);
                }
                employeeMaster.ProfileImage = uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool isUpdated = _employeeRepository.Update(employeeMaster);

                    if (!isUpdated)
                    {
                        return NotFound();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError("", "A concurrency error occurred while updating the record. Please try again.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An unexpected error occurred while updating the record. Please contact the administrator.");
                }
            }
            GetDrodown(employeeMaster);
            return View(employeeMaster);
        }

        // GET: Employees/Delete/5
        public IActionResult Delete(int id)
        {
            // Check if the id is null, if so, return a "Not Found" result
            if (id == 0)
            {
                return NotFound();
            }

            try
            {
                // Retrieve the employee record including related City, Country, and State data
                var employeeMaster = _employeeRepository.Get(id);  // Find the employee by id

                // If the employee record is not found, return a "Not Found" result
                if (employeeMaster == null)
                {
                    return NotFound();
                }

                // Return the employee details to the view for deletion confirmation
                return View(employeeMaster);
            }
            catch (Exception ex)
            {
                // Log the exception or handle accordingly               
                ModelState.AddModelError("", "An error occurred while retrieving the employee record. Please try again.");

                // Return a view that displays the error message
                return View(); // Assuming you have an "Error" view to display errors
            }
        }

        // POST: Employees/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var employeeMaster = _employeeRepository.Get(id);

                if (employeeMaster != null)
                {
                    _employeeRepository.Delete(id);
                }
                else
                {
                    return NotFound();
                }

                return Json(new { success = true, message = "Record deleted successfully." });
            }
            catch (DbUpdateException)
            {
                return Json(new { success = false, message = "An error occurred while deleting the record. Please try again." });
            }
            catch
            {
                return Json(new { success = false, message = "An unexpected error occurred. Please try again later." });
            }
        }


        // Method to check if an employee record exists by ID
        private bool EmployeeMasterExists(int id)
        {
            try
            {
                // Check if any employee record with the given ID exists in the database
                return _employeeRepository.GetAllEmployees().Any(e => e.Row_Id == id);
            }
            catch (Exception ex)
            {
                // Return false if an error occurs, indicating the record does not exist
                return false;
            }
        }

        // Method to populate dropdowns for countries, states, and cities
        [HttpGet]
        private void GetDrodown(EmployeeMaster employeeMaster)
        {
            try
            {
                // Fetch the list of countries and map them to SelectListItem objects
                var countries = GetCountry().Select(c => new SelectListItem
                {
                    Value = c.Row_Id.ToString(),
                    Text = c.CountryName
                }).ToList();

                // Fetch states only if CountryId is not null
                var states = employeeMaster?.CountryId != null
                    ? GetState(employeeMaster.CountryId).Select(s => new SelectListItem
                    {
                        Value = s.Row_Id.ToString(),
                        Text = s.StateName
                    }).ToList()
                    : new List<SelectListItem>();

                // Fetch cities only if StateId is not null
                var cities = employeeMaster?.StateId != null
                    ? GetCity(employeeMaster.StateId).Select(c => new SelectListItem
                    {
                        Value = c.Row_Id.ToString(),
                        Text = c.CityName
                    }).ToList()
                    : new List<SelectListItem>();

                ViewBag.CityId = cities;
                ViewBag.CountryId = countries;
                ViewBag.StateId = states;
            }
            catch (Exception)
            {
                // Add a generic error message to the ModelState
                ModelState.AddModelError("", "An error occurred while loading dropdown data. Please try again.");
            }
        }

        // Method to retrieve the list of all countries
        [HttpGet]
        public IList<Country> GetCountry()
        {
            return _countryRepository.GetAllCountries();
        }

        // Method to retrieve the list of states by country ID
        [HttpGet]
        public IList<State> GetState(int countryId)
        {
            return _stateRepository.GetAllStatesByCountryId(countryId);
        }

        // Method to retrieve the list of cities by state ID
        [HttpGet]
        public IList<City> GetCity(int stateId)
        {
            return _cityRepository.GetAllCitiesByStateId(stateId);
        }

        // Controller method to check if PAN Number is unique
        [HttpPost]
        public IActionResult CheckPanNumberUniqueness(string panNumber)
        {
            var isPanUnique = !_employeeRepository.GetAllEmployees().Any(e => e.PanNumber == panNumber);
            return Json(isPanUnique);
        }

        // Controller method to check if Passport Number is unique
        [HttpPost]
        public IActionResult CheckPassportNumberUniqueness(string passportNumber)
        {
            var isPassportUnique = !_employeeRepository.GetAllEmployees().Any(e => e.PassportNumber == passportNumber);
            return Json(isPassportUnique);
        }

        // Check if Email Address is unique
        [HttpPost]
        public IActionResult CheckEmailAddressUniqueness(string emailAddress)
        {
            var isEmailUnique = !_employeeRepository.GetAllEmployees().Any(e => e.EmailAddress == emailAddress);
            return Json(isEmailUnique);
        }

        // Check if Mobile Number is unique
        [HttpPost]
        public IActionResult CheckMobileNumberUniqueness(string mobileNumber)
        {
            var isMobileUnique = !_employeeRepository.GetAllEmployees().Any(e => e.MobileNumber == mobileNumber);
            return Json(isMobileUnique);
        }
    }
}