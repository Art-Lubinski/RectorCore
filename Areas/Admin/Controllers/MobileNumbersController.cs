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
    [Route("admin/accounts/cellnumbers")]
    public class MobileNumbersController : Controller
    {
        [Route("/accounts/cellnumbers")]
        public IActionResult Index()
        {
            ViewData["sidebar"] = "accounts";
            ViewData["WhereAmI"] = "Accounts";
            List<MobileNumber> numbers = new List<MobileNumber>();

            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();
                using (SqlCommand command =
                    new SqlCommand("SELECT MobileNumbers.*, Nodes.Name, Accounts.Provider, Accounts.AccountNumber FROM MobileNumbers LEFT JOIN Nodes ON MobileNumbers.ID = Nodes.PhoneNumberID LEFT JOIN Accounts ON MobileNumbers.AccountID = Accounts.ID ORDER BY provider", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    string[] lines = new string[] { };
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MobileNumber number = new MobileNumber();
                            number.number = reader.GetValue(reader.GetOrdinal("Number")).ToString();
                            number.plan = reader.GetValue(reader.GetOrdinal("Plan")).ToString();
                            number.provider = reader.GetValue(reader.GetOrdinal("Provider")).ToString();
                            number.accountId = reader.GetValue(reader.GetOrdinal("AccountID")).ToString();
                            number.accountNumber = reader.GetValue(reader.GetOrdinal("AccountNumber")).ToString();
                            number.nodeName = reader.GetValue(reader.GetOrdinal("Name")).ToString();
                            numbers.Add(number);

                        }

                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                }
            }
            return View(numbers);
        }


        [Route("/accounts/Edit")]
        public IActionResult Edit(string number, string provider, string plan, string account, string id)
        {
            ViewData["sidebar"] = "accounts";
            ViewData["WhereAmI"] = "Accounts";
            List<string> accounts = new List<string>();
            List<string> providers = new List<string>();
            MobileNumbersEditViewModel n = new MobileNumbersEditViewModel();
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();
                using (SqlCommand command =
                    new SqlCommand("SELECT AccountNumber FROM Accounts", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            accounts.Add(reader.GetValue(reader.GetOrdinal("AccountNumber")).ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();
                    command.CommandText = "SELECT DISTINCT(Provider) FROM Accounts";
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            providers.Add(reader.GetString(0));
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    reader.Close();

                    n.number = number;
                    n.accountNumber = account;
                    n.plan = plan;
                    n.provider = provider;
                    n.numberId = id;
                    n.providers = providers;
                    n.accounts = accounts;
                }
            }
            return View("Edit", n);
        }

    }

}
