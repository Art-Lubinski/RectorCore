using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using RectorCore.Models;
using RectorCore.ViewModels;
using RectorLocal;

namespace RectorCore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/node/{node}")]
    public class NodeController : Controller
    {
        [Route("")]
        [Route("/node/{node}")]
        public IActionResult Index(string node, string location= "NY")
        {
            ViewData["sidebar"] = "serverlist";
            ViewData["WhereAmI"] = node;
            ViewData["location"] = location;
            NodeInfoViewModel nodeObj = new NodeInfoViewModel();
            nodeObj.SelectFromDb(node);
            return View(nodeObj);
        }

        [Route("/node/{node}/Edit")]
        public IActionResult Edit(string location, string node)
        {
            ViewData["sidebar"] = "serverlist";
            ViewData["WhereAmI"] = node;
            ViewData["location"] = location;
            NodeInfoEditViewModel editNode = new NodeInfoEditViewModel();
            editNode.Name=node;
            editNode.SelectFromDb();

            return View("Edit", editNode);
        }

        [HttpPost]
        public IActionResult Save([Bind("NetworkLogin, NetworkPassword, TVPassword, AnydeskPassword, Status, PhoneNumberID")] NodeInfoEditViewModel model, string node)
        {
            model.Name = node;
            Debug.WriteLine($"{node} Name: {model.Name} PhoneID {model.PhoneNumberID} Status {model.Status} AnyDesk {model.AnydeskPassword} TV {model.TVPassword} NL {model.NetworkLogin}");
            if ( 1 == model.Save())
            {
                TempData["Result"] = "Record succesfully updated";
            }
            else
            {
                TempData["Result"] = "Error occurred. Database not updated";
            }
            return RedirectToAction("Index");
        }
    }
}