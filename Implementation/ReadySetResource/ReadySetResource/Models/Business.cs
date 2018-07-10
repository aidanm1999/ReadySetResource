
namespace ReadySetResource.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Business
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }
        public string Town { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string SecuriyNumber { get; set; }

        public string Plan { get; set; }

    }
}
