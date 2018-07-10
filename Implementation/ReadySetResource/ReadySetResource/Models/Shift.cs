using System.ComponentModel.DataAnnotations;
using System;


namespace ReadySetResource.Models
{


    public class Shift
    {

        [Key]
        public int Id { get; set; }



        public DateTime StartDateTime { get; set; } //Changed from integer to datetime

        public DateTime EndDateTime { get; set; } //Changed from integer to datetime

        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }
    }
}
