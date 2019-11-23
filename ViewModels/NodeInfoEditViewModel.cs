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
        public string Provider { get; set; }
        public string AnydeskPassword { get; set; }
        public string TVPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberID { get; set; }
        public List<SelectListItem> PhoneNumbers { get; set; }

        public SelectListGroup ATT { get; set; } = new SelectListGroup { Name = "ATT"};
        public SelectListGroup Verizon = new SelectListGroup { Name = "Verizon" };
        public SelectListGroup Sprint = new SelectListGroup { Name = "Sprint" };
        public SelectListGroup Other = new SelectListGroup { Name = "Other" };


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
                                   $"TVPassword='{TVPassword}', " +
                                   $"AnydeskPassword='{AnydeskPassword}' " +
                                   $"WHERE Name='{Name}'", connection))
                {
                    i = command.ExecuteNonQuery();
                    connection.Close();
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
                    new SqlCommand($"SELECT Number, Provider, A.Status, AnydeskPassword, TVPassword, PhoneNumberID FROM Nodes A LEFT JOIN MobileNumbers B ON A.PhoneNumberID = B.ID LEFT JOIN Accounts C ON B.AccountID = C.ID WHERE A.Name = '{Name}'", connection))
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

                            Provider = reader.GetValue(reader.GetOrdinal("Provider")).ToString();
                            AnydeskPassword = reader.GetValue(reader.GetOrdinal("AnydeskPassword")).ToString();
                            TVPassword = reader.GetValue(reader.GetOrdinal("TVPassword")).ToString();
                            PhoneNumber = reader.GetValue(reader.GetOrdinal("Number")).ToString();
                            PhoneNumberID = reader.GetValue(reader.GetOrdinal("PhoneNumberID")).ToString();
                        }
                    }
                    
                    reader.Close();
                    SelectListGroup group = new SelectListGroup();
                    PhoneNumbers = new List<SelectListItem>();
                    PhoneNumbers.Add(new SelectListItem { Value = "", Text = "", Selected = false });
                    if (PhoneNumber != "")
                    {
                        PhoneNumbers.Add(new SelectListItem { Value = PhoneNumberID, Text = PhoneNumber, Selected = true, Group = SelectGroup(Provider, group) });
                    }
                    command.CommandText = "SELECT A.ID, Number, Provider FROM MobileNumbers A LEFT JOIN Nodes B ON A.id = B.PhoneNumberID LEFT JOIN Accounts C ON A.AccountID = C.ID WHERE B.ID IS NULL";
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string provider = reader.GetValue(reader.GetOrdinal("Provider")).ToString();
                            string number = reader.GetValue(reader.GetOrdinal("Number")).ToString();
                            string id = reader.GetValue(reader.GetOrdinal("ID")).ToString();
                            bool isselected = false;
                            if (PhoneNumberID == id)
                            {
                                isselected = true;
                            }
                            Debug.WriteLine($"{id} {number} {isselected}");
                            SelectListItem item = new SelectListItem{Value = id, Text = number, Selected = isselected, Group = SelectGroup(provider, group)};
                            PhoneNumbers.Add(item);
                        }
                    }
                    reader.Close();
                }
            }
        }

        public SelectListGroup SelectGroup(string provider, SelectListGroup group)
        {
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

            return group;
        }
    }
}
