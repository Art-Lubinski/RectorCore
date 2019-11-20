using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RectorCore.Models;
using RectorCore.ViewModels;
using RectorLocal;

namespace RectorCore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/node/{location}/{node}")]
    public class NodeController : Controller
    {
        [Route("")]
        [Route("/node/{location}/{node}")]
        public IActionResult Index(string location, string node)
        {
            ViewData["sidebar"] = "serverlist";
            ViewData["WhereAmI"] = node;
            ViewData["location"] = location;
            Node nodeObj = DB.GetNode(node);
            UptimeWeek uw = DB.GetUptime(node);
            nodeObj.DownTime = uw.DownTime;
            nodeObj.ServiceInternet = uw.ServiceInternet;
            nodeObj.MainInternet = uw.MainInternet;
            nodeObj.DatesUptime = uw.Dates;
            DailyUsage du = DB.DataUsage("6");
            nodeObj.DatesUsage = du.Dates;
            nodeObj.Usage = du.Usage;
            return View(nodeObj);
        }

        //public IActionResult Edit(string node)
        //{
        //    ViewData["sidebar"] = "serverlist";
        //    ViewData["sidebar"] = "accounts";
        //    ViewData["WhereAmI"] = "Accounts";
        //    Node editNode = DB.GetNode(node);

        //    return View("Edit", editNode);
        //}
    }
}