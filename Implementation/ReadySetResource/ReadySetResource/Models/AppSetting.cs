using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
    public class AppSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string ClassName { get; set; }
        public string ControllerName { get; set; }

        public int AppId { get; set; }
        public App App { get; set; }
    }
}