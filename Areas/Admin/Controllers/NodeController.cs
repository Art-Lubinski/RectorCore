using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RectorCore.Models;
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
            Node nodeObj = new Node();
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {

                connection.Open();
                using (SqlCommand command =
                    new SqlCommand($"SELECT * FROM Nodes WHERE Name='{node}'", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            nodeObj.Name = reader.GetString(1);
                            nodeObj.RectorVersion = reader.GetString(2);
                            if (!reader.IsDBNull(3))
                            {
                                nodeObj.AnydeskID = reader.GetString(3);
                            }
                            if (!reader.IsDBNull(4))
                            {
                                nodeObj.AnydeskPassword = reader.GetString(4);
                            }
                            if (!reader.IsDBNull(5))
                            {
                                nodeObj.TVID = reader.GetString(5);
                            }
                            if (!reader.IsDBNull(6))
                            {
                                nodeObj.TVPassword = reader.GetString(6);
                            }
                            if (!reader.IsDBNull(7))
                            {
                                nodeObj.Is64bit = reader.GetBoolean(7);
                            }
                            if (!reader.IsDBNull(8))
                            {
                                nodeObj.OS = reader.GetString(8);
                            }
                            if (!reader.IsDBNull(9))
                            {
                                nodeObj.RAM = reader.GetDouble(9);
                            }
                            if (!reader.IsDBNull(10))
                            {
                                nodeObj.Storage = reader.GetDouble(10);
                            }
                            if (!reader.IsDBNull(11))
                            {
                                nodeObj.MemoryStick = reader.GetBoolean(11);
                            }
                            if (!reader.IsDBNull(12))
                            {
                                nodeObj.LocalAddress1 = reader.GetString(12);
                            }
                            if (!reader.IsDBNull(13))
                            {
                                nodeObj.LocalAddress2 = reader.GetString(13);
                            }
                            //nodeObj.LocalAddress2 = reader.GetString(13);
                            //nodeObj.Service = reader.GetString(14);
                            //nodeObj.Hub = reader.GetString(15);
                            //nodeObj.NodeNumber = reader.GetInt32(16);
                            //nodeObj.PhoneNumberID = reader.GetInt32(16);
                            //nodeObj.Model = reader.GetString(17);
                            //nodeObj.IsSSD = reader.GetBoolean(18);
                            //nodeObj.Modem = reader.GetString(19);
                            //nodeObj.NetworkAdapter = reader.GetString(20);
                        }
                    }

                    reader.Close();
                }

                return View(nodeObj);
            }
        }
    }
}