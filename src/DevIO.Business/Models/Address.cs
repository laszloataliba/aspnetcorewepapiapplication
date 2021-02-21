using System;

namespace DevIO.Business.Models
{
    public class Address : Entity
    {
        public Guid SupplierId { get; set; }
        public string StreetAddress { get; set; }
        public string Number { get; set; }
        public string AddressAddOn { get; set; }
        public string ZipCode { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public Supplier Supplier { get; set; }
    }
}
