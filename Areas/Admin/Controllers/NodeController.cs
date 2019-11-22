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

        [Route("/node/{location}/{node}/Edit")]
        public IActionResult Edit(string location, string node)
        {
            Debug.WriteLine("IN Edit");
            ViewData["sidebar"] = "serverlist";
            ViewData["sidebar"] = "accounts";
            ViewData["WhereAmI"] = "Accounts";
            ViewData["location"] = location;
            NodeInfoEditViewModel editNode = DB.GetNodeEdit(node);

            return View("Edit", editNode);
        }

        [Route("/node/Edit")]
        public IActionResult Save()
        {
            Debug.WriteLine("IN SAVE");
            NodeInfoEditViewModel editNode = new NodeInfoEditViewModel();
            editNode.Status = HttpContext.Request.Form["SelectStatus"].ToString();
            
            if (editNode.SaveNodeInfo() != 0)
            {
                TempData["Result"] = "Data saved Successfully";
            }
            else
            {
                TempData["Result"] = "Fail M&ZAF%KA!";
            }

            return RedirectToAction("Index");
        }
    }
}