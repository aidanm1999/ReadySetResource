using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadySetResource.Models;
using System.Web.Mvc;

namespace ReadySetResource.ViewModels
{
    public class AddShiftViewModel
    {
        public Shift Shift { get; set; }
        public List<SelectListItem> Employees { get; set; }
        public List<Holiday> EmployeesHolidays { get; set; }
    }
}