using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{


    public class EmployeeType
    {

        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(70)]
        [MinLength(3)]
        public string Title { get; set; }


        [MaxLength(255)]
        public string Description { get; set; }


        public int BaseSalary { get; set; }


    }
}
