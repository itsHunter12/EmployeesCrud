using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeesCrud.Models
{
    public class State
    {
        [Key]
        public int Row_Id { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string StateName { get; set; }

        public virtual Country Country { get; set; }

        // Navigation property for lazy loading
        //public virtual ICollection<City> Cities { get; set; }
    }
}
