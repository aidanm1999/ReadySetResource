using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;

namespace ReadySetResource.ViewModels
{
    public class AddShiftViewModel
    {
        public Shift Shift { get; set; }
        public List<ApplicationUser> Employees { get; set; }
        public List<Holiday> EmployeesHolidays { get; set; }
        public ApplicationUser SelectedEmployee { get; set; }
    }
}