//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReadySetResource.Models
{
<<<<<<< HEAD
    using System;
    using System.Collections.Generic;
    
    public partial class Holiday
    {
        public int Id { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public System.DateTime EndDateTime { get; set; }
        public string Accepted { get; set; }
        public string UserId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
=======
    /// <summary>
    /// Creates an instance of the holiday when called upon
    /// </summary>
    public class Holiday
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the start date time.
        /// </summary>
        /// <value>
        /// The start date time.
        /// </value>
        [Required]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the end date time.
        /// </summary>
        /// <value>
        /// The end date time.
        /// </value>
        [Required]
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the accepted.
        /// </summary>
        /// <value>
        /// The accepted.
        /// </value>
        [Required]
        public string Accepted { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public ApplicationUser User { get; set; }


>>>>>>> parent of ae2ad3a... Took out XML Comments
    }
}
