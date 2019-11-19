using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RectorCore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/dashboard")]
    public class DashboardController : Controller
    {
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewData["sidebar"] = "overview";
            ViewData["WhereAmI"] = "Overview";
            return View();
        }
    }
}