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
    }
}
