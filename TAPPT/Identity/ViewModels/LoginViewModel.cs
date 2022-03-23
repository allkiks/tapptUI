using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Identity.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email Address:")]
        public string EmailAddress { get; set; }
        [Required]
        [Display(Name = "Login Password:")]
        public string Password { get; set; }

        [Display(Name = "Remember Me:")]
        public bool RememberMe { get; set; }
    }
}
