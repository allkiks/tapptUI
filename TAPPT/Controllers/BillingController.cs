using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TAPPT.Web.Controllers
{
    [Authorize]
    public class BillingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Billing()
        {
            return View();
        }
        public IActionResult Invoice()
        {
            return View();
        }
    }
}