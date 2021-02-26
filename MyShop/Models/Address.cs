
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Address
    {
        public Address()
        {

        }

        public int AddressId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string Details { get; set; }
        public string MyUserId { get; set; }
        public ApplicationUser MyUser { get; set; }
        public string UserFullName { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverMobileNumber { get; set; }
        public string PostalCode { get; set; }
        public bool IsSelected { get; set; }

    }
}
