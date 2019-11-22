using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RectorLocal;
using Remotion.Linq.Clauses;

namespace RectorCore.ViewModels
{
    public class NodeInfoEditViewModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string AnydeskPassword { get; set; }
        public string TVPassword { get; set; }
        public string PhoneNumberID { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public List<string> PhoneNumbersIDS { get; set; }
        public string NetworkLogin { get; set; }
        public string NetworkPassword { get; set; }

        public int SaveNodeInfo()
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();
                using (SqlCommand command =
                    new SqlCommand($"UPDATE Nodes SET Status='{Status}' WHERE Name='{Name}'", connection))
                {
                    i = command.ExecuteNonQuery();
                    connection.Close();
                    Debug.WriteLine($"RESULT FROM UPDATE {i}");
                }

            }
            return i;
        }
        public void GetNodeSelectFromDb(string node)
        {
            List<string> phonenumbers = new List<string>();
            List<string> phoneids = new List<string>();
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {

                connection.Open();
                using (SqlCommand command =
                    new SqlCommand($"SELECT Number, Status, AnydeskPassword, TVPassword, NetworkLogin, NetworkPassword FROM Nodes LEFT JOIN MobileNumbers ON Nodes.PhoneNumberID = MobileNumbers.ID WHERE Nodes.Name = '{node}'", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Name = node;
                            Status = reader.GetValue(reader.GetOrdinal("Status")).ToString();
                            AnydeskPassword = reader.GetValue(reader.GetOrdinal("AnydeskPassword")).ToString();
                            TVPassword = reader.GetValue(reader.GetOrdinal("TVPassword")).ToString();
                            PhoneNumber = reader.GetValue(reader.GetOrdinal("Number")).ToString();
                            NetworkLogin = reader.GetValue(reader.GetOrdinal("NetworkLogin")).ToString();
                            NetworkPassword = reader.GetValue(reader.GetOrdinal("NetworkPassword")).ToString();
                        }
                    }
                    reader.Close();
                    command.CommandText = "Select Number, ID FROM MobileNumbers";
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            phoneids.Add(reader.GetValue(reader.GetOrdinal("ID")).ToString());
                            phonenumbers.Add(reader.GetValue(reader.GetOrdinal("Number")).ToString());
                        }
                    }
                    reader.Close();
                }

                PhoneNumbersIDS = phoneids;
                PhoneNumbers = phonenumbers;
            }
        }
    }
}
