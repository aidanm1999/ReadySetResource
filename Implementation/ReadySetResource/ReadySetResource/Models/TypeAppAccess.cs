using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReadySetResource.Models
{
    public class TypeAppAccess
    {
        [Key]
        public int Id { get; set; }

        public string AccessType { get; set; }

        public int AppId { get; set; }
        public App App { get; set; }

        public int BusinessUserTypeId { get; set; }
        public BusinessUserType BusinessUserType { get; set; }
    }
}