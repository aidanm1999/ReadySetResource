﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;

namespace ReadySetResource.ViewModels.Account
{
    /// <summary>
    /// Creates an instance of the inviteviewmodel when called upon
    /// </summary>
    public class InviteViewModel
    {
        /// <summary>
        /// Gets or sets the new user.
        /// </summary>
        /// <value>
        /// The new user.
        /// </value>
        public ApplicationUser NewUser { get; set; }
        /// <summary>
        /// Gets or sets the temporary pass.
        /// </summary>
        /// <value>
        /// The temporary pass.
        /// </value>
        public string TempPass { get; set; }
        /// <summary>
        /// Gets or sets the invite code.
        /// </summary>
        /// <value>
        /// The invite code.
        /// </value>
        public string InviteCode { get; set; }
    }
}