using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RectorCore.Models;
using RectorLocal;

namespace RectorCore.ViewModels
{
    public class NodesViewModel
    {
        public List<NodeCustomerViewModel> NodeCustomerViewMobile { get; set; } = new List<NodeCustomerViewModel>();
        public List<NodeCustomerViewModel> NodeCustomerViewDSL { get; set; } = new List<NodeCustomerViewModel>();

        public void Select(string location)
        {
            
            string commndStr = GetSelectCommand(location);

            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();
                using (SqlCommand command =
                    new SqlCommand(commndStr, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    string[] lines = new string[] { };
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            NodeCustomerViewModel ncvm = new NodeCustomerViewModel();
                            ncvm.Name = reader.GetValue(reader.GetOrdinal("Name")).ToString();
                            ncvm.Service = reader.GetValue(reader.GetOrdinal("Service")).ToString();
                            ncvm.Provider = reader.GetValue(reader.GetOrdinal("Provider")).ToString();
                            if(ncvm.Service == "DSL")
                            {
                                NodeCustomerViewDSL.Add(ncvm);
                            }
                            else
                            {
                                NodeCustomerViewMobile.Add(ncvm);
                            }
                        }
                    }
                    reader.Close();
                }
            }
        }

        private string GetSelectCommand(string state)
        {

            string command;
            if (state == "NY")
            {
                command = "SELECT Name, Service, C.Provider FROM Nodes A LEFT JOIN MobileNumbers B ON A.PhoneNumberID = B.ID LEFT JOIN Accounts C ON B.AccountID = C.ID WHERE Hub='NY222' OR Hub='NY22' OR Hub='NY2' ORDER BY Name";
            }
            else
            {
                command = $"SELECT Name, Service, C.Provider FROM Nodes A LEFT JOIN MobileNumbers B ON A.PhoneNumberID = B.ID LEFT JOIN Accounts C ON B.AccountID = C.ID WHERE Hub='{state}' ORDER BY Name";
            }
            return command;
        }
    }
}
