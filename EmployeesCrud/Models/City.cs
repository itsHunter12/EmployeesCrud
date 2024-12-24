using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeesCrud.Models
{
    public class City
    {
        [Key]
        public int Row_Id { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CityName { get; set; }

        public virtual State State { get; set; }
    }

}
