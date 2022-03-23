using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace TAPPT.Web.Helpers
{
    public static class Security
    {
        public static bool IsInGroup(ClaimsPrincipal User, string GroupName)
        {
            bool check = false;
            var user = (WindowsIdentity)User.Identity;
            if (user.Groups != null)
            {
                foreach (var group in user.Groups)
                {
                    check = group.Translate(typeof(NTAccount)).ToString().Contains(GroupName);
                    if (check)
                        break;
                }
            }
            return check;
        }
    }
}
