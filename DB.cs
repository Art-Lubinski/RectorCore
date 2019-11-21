using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using RectorCore.Models;
using RectorCore.ViewModels;
using RectorLocal;

namespace RectorCore
{
    public static class DB
    {
        public static NodeInfoViewModel GetNode(string node)
        {
            NodeInfoViewModel nodeObj = new NodeInfoViewModel();
            
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
                            nodeObj.Name = DBUtills.SafeGetString(reader, 1);
                            nodeObj.RectorVersion = DBUtills.SafeGetString(reader, 2);
                            nodeObj.AnydeskID = DBUtills.SafeGetString(reader, 3);
                            nodeObj.NetworkLogin = reader.GetValue(reader.GetOrdinal("NetworkLogin")).ToString();
                            nodeObj.NetworkPassword = reader.GetValue(reader.GetOrdinal("NetworkPassword")).ToString();
                            DBUtills.SafeGetString(reader, 4);
                            if (!reader.IsDBNull(5))
                            {
                                nodeObj.TVID = DBUtills.SafeGetString(reader, 5);
                            }

                            nodeObj.TVPassword = DBUtills.SafeGetString(reader, 6);
                            if (!reader.IsDBNull(7))
                            {
                                nodeObj.Is64bit = reader.GetBoolean(7);
                            }

                            nodeObj.OS = DBUtills.SafeGetString(reader, 8);
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

                            nodeObj.LocalAddress1 = DBUtills.SafeGetString(reader, 12);
                            nodeObj.LocalAddress2 = DBUtills.SafeGetString(reader, 13);
                            nodeObj.Service = DBUtills.SafeGetString(reader, 14);
                            nodeObj.Hub = DBUtills.SafeGetString(reader, 15);
                            if (!reader.IsDBNull(16))
                            {
                                nodeObj.NodeNumber = reader.GetInt32(16);
                            }

                            if (!reader.IsDBNull(17))
                            {
                                nodeObj.PhoneNumberID = reader.GetInt32(17);
                            }

                            nodeObj.Model = DBUtills.SafeGetString(reader, 18);
                            if (!reader.IsDBNull(19))
                            {
                                nodeObj.IsSSD = reader.GetBoolean(19);
                            }

                            nodeObj.Modem = DBUtills.SafeGetString(reader, 20);
                            if (!reader.IsDBNull(21))
                            {
                                nodeObj.NetworkAdapter = reader.GetBoolean(21);
                            }
                        }
                    }

                    reader.Close();
                }

                return nodeObj;
            }
        }

        public static NodeInfoEditViewModel GetNodeEdit(string node)
        {
            NodeInfoEditViewModel nodeObj = new NodeInfoEditViewModel();
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
                            nodeObj.Name = node;
                            nodeObj.Status = reader.GetValue(reader.GetOrdinal("Status")).ToString();
                            nodeObj.AnydeskPassword = reader.GetValue(reader.GetOrdinal("AnydeskPassword")).ToString();
                            nodeObj.TVPassword = reader.GetValue(reader.GetOrdinal("TVPassword")).ToString();
                            nodeObj.PhoneNumber = reader.GetValue(reader.GetOrdinal("Number")).ToString();
                            nodeObj.NetworkLogin = reader.GetValue(reader.GetOrdinal("NetworkLogin")).ToString();
                            nodeObj.NetworkPassword = reader.GetValue(reader.GetOrdinal("NetworkPassword")).ToString();
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

                nodeObj.PhoneNumbersIDS = phoneids;
                nodeObj.PhoneNumbers = phonenumbers;
            }

            return nodeObj;
        }

        public static UptimeWeek GetUptime(string nodeName)
        {
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            List<double> z = new List<double>();
            List<string> date = new List<string>();
            int tmp = 0;
            double ServInternet = 0;
            double MainInternet = 0;
            double DownTime = 0;
            DateTime to = DateTime.Now;
            DateTime from = to.AddDays(-7);
            to = from.AddDays(1);
            int i;
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();
                for (i = 0; i < 7; i++)
                { 
                    using (SqlCommand command =
                        new SqlCommand($"SELECT COUNT(ServiceInternet) From NodesUptime WHERE NodeName='{nodeName}'AND ServiceInternet=1 and Date BETWEEN '{from.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{to.ToString("yyyy-MM-dd HH:mm:ss")}' ", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tmp = reader.GetInt32(0);
                                ServInternet = (tmp * 100 / 288);
                                x.Add(ServInternet);
                            }
                        }
                        reader.Close();
                        tmp = 0;
                        command.CommandText =
                            $"SELECT COUNT(ServiceInternet) From NodesUptime WHERE NodeName='{nodeName}'AND ServiceInternet=0 and Date BETWEEN '{from}' AND '{to}' ";

                        reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tmp = reader.GetInt32(0);
                                MainInternet = (tmp * 100 / 288);
                                y.Add(MainInternet);
                            }
                        }
                        reader.Close();
                    }
                    date.Add(to.ToShortDateString());
                    from = from.AddDays(1);
                    to = to.AddDays(1);
                }
            }

            var numbersAndWords = x.Zip(y, (n, w) => new { Serv = n, Main = w });
            foreach (var nw in numbersAndWords)
            {
                z.Add((100-nw.Serv - nw.Main));
            }
            UptimeWeek uw = new UptimeWeek();
            uw.ServiceInternet = x;
            uw.MainInternet = y;
            uw.DownTime = z;
            uw.Dates = date;
            return uw;
        }
        public static DailyUsage DataUsage(string phonenumberID)
        {
            List<double> dailyUsage = new List<double>();
            List<string> date = new List<string>();
            double temp1;
            DateTime temp2;
            DateTime to = DateTime.Now;
            DateTime from = to.AddDays(-30);
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();
                
                using (SqlCommand command =
                    new SqlCommand($"SELECT Usage, Date From UsageHistory WHERE ID={phonenumberID} and Date BETWEEN '{from.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{to.ToString("yyyy-MM-dd HH:mm:ss")}' ", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            temp1 = reader.GetDouble(0);
                            temp2 = reader.GetDateTime(1);
                            dailyUsage.Add(temp1);
                            date.Add(temp2.ToShortDateString());
                        }
                    }
                    reader.Close();
                }
            }
            DailyUsage du = new DailyUsage();
            du.Dates = date;
            du.Usage = dailyUsage;
            return du;
        }
    }
}