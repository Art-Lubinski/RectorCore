using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RectorLocal;
using Remotion.Linq.Clauses;

namespace RectorCore.ViewModels
{
    public class NodeInfoEditViewModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public List<SelectListItem> StatusListItems { get; set; } 
        public string AnydeskPassword { get; set; }
        public string TVPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberID { get; set; }
        public List<SelectListItem> PhoneNumbers { get; set; }
        public string NetworkLogin { get; set; }
        public string NetworkPassword { get; set; }

        public int Save()
        {
            if (Status == "0")
            {
                Status = "Broken";

            }

            if (Status == "1")
            {
                Status = "Active";

            }
            int i = 0;
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();

                using (SqlCommand command =
                    new SqlCommand($"UPDATE Nodes SET Status='{Status}', " +
                                   $"PhoneNumberID='{PhoneNumberID}', " +
                                   $"NetworkLogin='{NetworkLogin}', " +
                                   $"NetworkPassword='{NetworkPassword}', " +
                                   $"TVPassword='{TVPassword}', " +
                                   $"AnydeskPassword='{AnydeskPassword}' " +
                                   $"WHERE Name='{Name}'", connection))
                {
                    i = command.ExecuteNonQuery();
                    connection.Close();
                    Debug.WriteLine($"RESULT FROM UPDATE {i}");
                }

            }
            return i;
        }
        public void SelectFromDb()
        {
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();
                using (SqlCommand command =
                    new SqlCommand($"SELECT Number, Status, AnydeskPassword, TVPassword, NetworkLogin, NetworkPassword, PhoneNumberID FROM Nodes LEFT JOIN MobileNumbers ON Nodes.PhoneNumberID = MobileNumbers.ID WHERE Nodes.Name = '{Name}'", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            bool isactive = false;
                            bool isbroken = false;
                            string status = reader.GetValue(reader.GetOrdinal("Status")).ToString();
                            if (status == "Active") isactive = true;
                            if (status == "Broken") isbroken = true;
                            StatusListItems = new List<SelectListItem>
                            {
                                new SelectListItem {Value = "1", Text = "Active", Selected = isactive},
                                new SelectListItem {Value = "0", Text = "Broken", Selected = isbroken}
                            };
                            AnydeskPassword = reader.GetValue(reader.GetOrdinal("AnydeskPassword")).ToString();
                            TVPassword = reader.GetValue(reader.GetOrdinal("TVPassword")).ToString();
                            PhoneNumber = reader.GetValue(reader.GetOrdinal("Number")).ToString();
                            PhoneNumberID = reader.GetValue(reader.GetOrdinal("PhoneNumberID")).ToString();
                            NetworkLogin = reader.GetValue(reader.GetOrdinal("NetworkLogin")).ToString();
                            NetworkPassword = reader.GetValue(reader.GetOrdinal("NetworkPassword")).ToString();
                        }
                    }
                    
                    reader.Close();
                    var Verizon = new SelectListGroup { Name = "Verizon" };
                    var ATT = new SelectListGroup { Name = "ATT" };
                    var Sprint = new SelectListGroup { Name = "Sprint" };
                    var Other = new SelectListGroup { Name = "Other" };
                    SelectListGroup group = new SelectListGroup();
                    bool notselected = false;
                    command.CommandText = "SELECT A.ID, Number, Provider FROM MobileNumbers A LEFT JOIN Nodes B ON A.id = B.PhoneNumberID LEFT JOIN MobileAccounts C ON A.AccountID = C.ID WHERE B.ID IS NULL";
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        PhoneNumbers = new List<SelectListItem>();
                        while (reader.Read())
                        {
                            
                            string provider = reader.GetValue(reader.GetOrdinal("Provider")).ToString();
                            string number = reader.GetValue(reader.GetOrdinal("Number")).ToString();
                            string id = reader.GetValue(reader.GetOrdinal("ID")).ToString();
                            bool isselected = false;
                            if (provider == "Verizon")
                            {
                                group = Verizon;
                            }
                            else if (provider == "ATT")
                            {
                                group = ATT;
                            }
                            else if (provider == "Sprint")
                            {
                                group = Sprint;
                            }
                            else
                            {
                                group = Other;
                            }

                            if (PhoneNumberID == id)
                            {
                                isselected = true;
                                notselected = true;
                            }
                            SelectListItem item = new SelectListItem{Value = id, Text = number, Selected = isselected, Group = group};
                            PhoneNumbers.Add(item);
                        }

                        if (!notselected)
                        {
                            PhoneNumbers.Add(new SelectListItem { Value = "placeholder", Text = "Select number", Selected = true});
                        }
                    }
                    reader.Close();
                }
            }
        }
    }
}
