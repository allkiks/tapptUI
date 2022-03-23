using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TAPPT.Web.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddDevice()
        {
            return View();
        }

        public IActionResult SearchDevice()
        {
            return View();
        }
    }
}