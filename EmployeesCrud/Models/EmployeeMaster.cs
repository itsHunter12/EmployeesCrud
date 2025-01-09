using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeesCrud.Models
{
    public class EmployeeMaster
    {
        [Key]
        public int Row_Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "Please enter the employee's First name.")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [ForeignKey("Country")]
        [Required(ErrorMessage = "Please select a country.")]
        public int CountryId { get; set; }

        [ForeignKey("State")]
        [Required(ErrorMessage = "Please select a state.")]
        public int StateId { get; set; }

        [ForeignKey("City")]
        [Required(ErrorMessage = "Please select a city.")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter Mobile number")]
        [MaxLength(15)]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter PAN")]
        [MaxLength(12)]
        public string PanNumber { get; set; }

        [Required(ErrorMessage = "Please enter Passport number")]
        [MaxLength(20)]
        public string PassportNumber { get; set; }

        [MaxLength(100)]
        public string ProfileImage { get; set; }
        [Required(ErrorMessage = "Please select Gender")]
        public byte Gender { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Please enter DOB")]
        public DateOnly DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please enter DOJ")]
        public DateOnly? DateOfJoinee { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedDate { get; set; }
        [NotMapped]
        public IFormFile UploadedFile { get; set; }

        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public virtual City City { get; set; }
    }
}
