using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Identity.Models
{
    public enum Roles
    {
        [Description("Operator Admin")]
        Admin,
        [Description("WSP Admin")]
        WSPAdmin,
        [Description("WSP Clark")]
        WSPUser,
        [Description("Customer")]
        Customer,
        [Description("Operator Management")]
        Management,
        [Description("Operator Support")]
        Support,
        [Description("Customer Admin")]
        CustomerAdmin,
        [Description("Customer Management")]
        CustomerManagement
    }
}
