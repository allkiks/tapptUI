using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using TAPPT.Web.Identity.Models;
using System;

namespace TAPPT.Web.Identity.Extensions
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("DisplayName", user.DisplayName ?? string.Empty));
            identity.AddClaim(new Claim("Email", user.Email ?? string.Empty));
            identity.AddClaim(new Claim("Id", user.Id ?? string.Empty));
            identity.AddClaim(new Claim("LogoUrl", user.LogoUrl ?? string.Empty));
            identity.AddClaim(new Claim("WSPId", user.WSPId ?? string.Empty));
            return identity;
        }
    }
}
