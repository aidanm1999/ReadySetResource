using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ReadySetResource.Models;

namespace ReadySetResource.Models
{
    /// <summary>
    /// Creates an instance of the password when called upon
    /// </summary>
    public class Password
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
        /// Gets or sets the pass string.
        /// </summary>
        /// <value>
        /// The pass string.
        /// </value>
        [Required]
        [MaxLength(20)] [MinLength(8)]
        [RegularExpression("(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9])$", ErrorMessage = "Password must have a letter, a number and no special characters.")]
        public string PassString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Password"/> is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if valid; otherwise, <c>false</c>.
        /// </value>
        [Required]
        public bool Valid { get; set; }

        //Foreign Key
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public ApplicationUser User { get; set; }


    }
}