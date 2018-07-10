//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the employee type




using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

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