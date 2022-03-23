using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TAPPT.Web.Controllers
{
    [Authorize]
    public class HelpController : Controller
    {
        public IActionResult Support()
        {
            return View();
        }
       
    }
}