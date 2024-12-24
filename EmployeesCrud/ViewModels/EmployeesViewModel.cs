namespace EmployeesCrud.ViewModels
{
    public class EmployeesViewModel
    {
        public string EmployeeCode { get; set; }
        public int Row_Id { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; } 

        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string PanNumber { get; set; }
        public string PassportNumber { get; set; }

        public string ProfileImage { get; set; }
        public byte Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
