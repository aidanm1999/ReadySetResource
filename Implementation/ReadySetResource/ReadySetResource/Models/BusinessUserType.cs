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
    
    public partial class BusinessUserType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BusinessUserType()
        {
            this.AspNetUsers = new HashSet<AspNetUser>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int BusinessId { get; set; }
        public string Administrator { get; set; }
        public string Updates { get; set; }
        public string Store { get; set; }
        public string Messenger { get; set; }
        public string Meetings { get; set; }
        public string Calendar { get; set; }
        public string Holidays { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public virtual Business Business { get; set; }
=======
    /// <summary>
    /// Creates an instance of the businessusertype when called upon
    /// </summary>
    public class BusinessUserType
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the administrator.
        /// </summary>
        /// <value>
        /// The administrator.
        /// </value>
        [Required]
        public string Administrator { get; set; }

        /// <summary>
        /// Gets or sets the calendar.
        /// </summary>
        /// <value>
        /// The calendar.
        /// </value>
        [Required]
        public string Calendar { get; set; }

        /// <summary>
        /// Gets or sets the updates.
        /// </summary>
        /// <value>
        /// The updates.
        /// </value>
        [Required]
        public string Updates { get; set; }

        /// <summary>
        /// Gets or sets the store.
        /// </summary>
        /// <value>
        /// The store.
        /// </value>
        [Required]
        public string Store { get; set; }

        /// <summary>
        /// Gets or sets the messenger.
        /// </summary>
        /// <value>
        /// The messenger.
        /// </value>
        [Required]
        public string Messenger { get; set; }

        /// <summary>
        /// Gets or sets the meetings.
        /// </summary>
        /// <value>
        /// The meetings.
        /// </value>
        [Required]
        public string Meetings { get; set; }

        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>
        /// The holidays.
        /// </value>
        [Required]
        public string Holidays { get; set; }

        /// <summary>
        /// Gets or sets the business identifier.
        /// </summary>
        /// <value>
        /// The business identifier.
        /// </value>
        public int BusinessId { get; set; }
        /// <summary>
        /// Gets or sets the business.
        /// </summary>
        /// <value>
        /// The business.
        /// </value>
        public Business Business { get; set; }
>>>>>>> parent of ae2ad3a... Took out XML Comments
    }
}
