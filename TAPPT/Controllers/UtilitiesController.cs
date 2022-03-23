using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TAPPT.Web.Controllers
{
    [Authorize]
    public class UtilitiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Tarrifs()
        {
            return View();
        }
        public IActionResult ViewTarrif()
        {
            return View();
        }
        public IActionResult AddTarrif()
        {
            return View();
        }
    }
}