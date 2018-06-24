//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the data over time




using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
    /// <summary>
    /// Creates an instance of the data over time when called upon
    /// </summary>
    public class DataOverTime
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
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        [Required]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the memory mb.
        /// </summary>
        /// <value>
        /// The memory mb.
        /// </value>
        [Required]
        public float MemoryMB { get; set; }

        /// <summary>
        /// Gets or sets the business.
        /// </summary>
        /// <value>
        /// The business.
        /// </value>
        public Business Business { get; set; }
    }
}