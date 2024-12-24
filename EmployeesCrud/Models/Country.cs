using System.ComponentModel.DataAnnotations;

namespace EmployeesCrud.Models
{
    public class Country
    {
        [Key]
        public int Row_Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CountryName { get; set; }

        //public virtual ICollection<State> States { get; set; }
    }
}
