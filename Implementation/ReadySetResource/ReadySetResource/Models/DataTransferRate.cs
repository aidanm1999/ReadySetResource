//Document Author:      Aidan Marshall
//Date Created:         20/3/2018
//Date Last Modified:   8/6/2018
//Description:          This model deals with holding all details for the data transfer rate





using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System;

namespace ReadySetResource.Models
{
    /// <summary>
    /// Creates an instance of the data transfer rate when called upon
    /// </summary>
    public class DataTransferRate
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
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the start page.
        /// </summary>
        /// <value>
        /// The start page.
        /// </value>
        [Required]
        public string StartPage { get; set; }

        /// <summary>
        /// Gets or sets the end page.
        /// </summary>
        /// <value>
        /// The end page.
        /// </value>
        public string EndPage { get; set; }
    }
}