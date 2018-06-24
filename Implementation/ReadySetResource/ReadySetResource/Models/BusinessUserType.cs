//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the Business User Type






using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
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
    }
}