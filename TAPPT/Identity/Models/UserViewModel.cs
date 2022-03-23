using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Identity.Models
{
    public class UserViewModel
    {
        private readonly ApplicationUser _applicationUser;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserViewModel(ApplicationUser applicationUser, RoleManager<IdentityRole> roleManager)
        {
            _applicationUser = applicationUser;
            _roleManager = roleManager;
        }
        public string Id
        {
            get
            {
                return _applicationUser.Id;
            }
        }
        public string DisplayName
        {
            get
            {
                return _applicationUser.DisplayName;
            }
        }
        public string Email
        {
            get
            {
                return _applicationUser.Email;
            }
        }
        public string Role
        {
            get
            {
                return _applicationUser.RoleName;
            }
        }
    }
}
