﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;

namespace ReadySetResource.ViewModels
{
    public class ManagerDetailsViewModel
    {
        public ApplicationUser NewManager { get; set; }
        public Business NewBusiness { get; set; }
    }
}