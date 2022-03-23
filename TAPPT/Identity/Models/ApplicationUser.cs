using Microsoft.AspNetCore.Identity;

namespace TAPPT.Web.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string LogoUrl { get; set; }
        public string Residence { get; set; }
        public string PostalAddress { get; set; }
        public string IdNumber { get; set; }
        public string RoleName { get; set; }
        public string WSPId { get; set; }
        public string DisplayName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
