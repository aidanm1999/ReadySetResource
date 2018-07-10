using System;
using System.ComponentModel.DataAnnotations;


namespace ReadySetResource.Models
{
    public class Message
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
        /// Gets or sets the date time send.
        /// </summary>
        /// <value>
        /// The date time send.
        /// </value>
        [Required]
        public DateTime DateTimeSend { get; set; }

        /// <summary>
        /// Gets or sets the date time viewed.
        /// </summary>
        /// <value>
        /// The date time viewed.
        /// </value>
        public DateTime DateTimeViewed { get; set; }


        /// <summary>
        /// Gets or sets the content string.
        /// </summary>
        /// <value>
        /// The content string.
        /// </value>
        public string ContentString { get; set; }


        /// <summary>
        /// Gets or sets the content image path.
        /// </summary>
        /// <value>
        /// The content image path.
        /// </value>
        public string ContentImagePath { get; set; }


        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public ApplicationUser Sender { get; set; }

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>
        /// The recipient.
        /// </value>
        public ApplicationUser Recipient { get; set; }
    }
}
