using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;
using RectorCore.Models;
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
            List<Node> nodes = new List<Node>();
            
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();
                using (SqlCommand command =
                    new SqlCommand("SELECT Name FROM Nodes WHERE Hub='NY222' OR Hub='NY22' OR Hub='NY2' ORDER BY Name", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    string[] lines = new string[]{};
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Node node = new Node();
                            node.Name = reader.GetString(0);
                            nodes.Add(node);
                        }

                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return View(nodes);
        }
    }

}
