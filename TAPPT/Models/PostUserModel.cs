using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class PostUserModel
    {
        public int WSPUtilityId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PostalAddress { get; set; }
        public string Residence { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public string Group { get; set; }
        public string IdentityId { get; set; }
    }
}
