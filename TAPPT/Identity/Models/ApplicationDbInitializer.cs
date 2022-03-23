using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Identity.Models
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUpepoAdmin(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@upepo.io").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = "Upepo",
                    LastName = "Admin",
                    RoleName = "Admin",
                    UserName = "admin@upepo.io",
                    Email = "admin@upepo.io"
                };

                var result = userManager.CreateAsync(user, "#Karibu2019").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
        public static void SeedSafcomAdmin(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@safaricom.co.ke").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = "Safaricom",
                    LastName = "Admin",
                    RoleName = "Admin",
                    UserName = "admin@safaricom.co.ke",
                    Email = "admin@safaricom.co.ke"
                };

                var result = userManager.CreateAsync(user, "#Karibu2019").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
