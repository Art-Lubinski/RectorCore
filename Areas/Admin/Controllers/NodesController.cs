using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;
using RectorCore.Models;
using RectorCore.ViewModels;
using RectorLocal;

namespace RectorCore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/nodes")]
    public class NodesController : Controller
    {
        [Route("")]
        [Route("/{location}")]
        public IActionResult Index(string location = "NY")
        {
            ViewData["sidebar"] = "serverlist";
            ViewData["WhereAmI"] = "Server List";
            ViewData["location"] = location;
            NodesViewModel viewModel = new NodesViewModel();
            viewModel.Select(location);
            return View(viewModel);
        }
    }

}
