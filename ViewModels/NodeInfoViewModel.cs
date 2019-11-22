using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RectorLocal;

namespace RectorCore.ViewModels
{
    public class NodeInfoViewModel
    {
        public string Name { get; set; }
        public string RectorVersion { get; set; }
        public string AnydeskID { get; set; }
        public string AnydeskPassword { get; set; }
        public string TVID { get; set; }
        public string TVPassword { get; set; }
        public bool Is64bit { get; set; }
        public string OS { get; set; }
        public double RAM { get; set; }
        public double Storage { get; set; }
        public bool MemoryStick { get; set; }
        public string LocalAddress1 { get; set; }
        public string LocalAddress2 { get; set; }
        public string Service { get; set; }
        public string Hub { get; set; }
        public Int32 NodeNumber { get; set; }
        public string PhoneNumberID { get; set; }
        public string Model { get; set; }
        public bool IsSSD { get; set; }
        public string Modem { get; set; }
        public bool NetworkAdapter { get; set; }
        public List<double> ServiceInternet { get; set; }
        public List<double> MainInternet { get; set; }
        public List<double> DownTime { get; set; }
        public List<string> DatesUptime { get; set; }
        public List<double> Usage { get; set; }
        public List<string> DatesUsage { get; set; }
        public string NetworkLogin { get; set; }
        public string NetworkPassword { get; set; }
        public double TotalUsed { get; set; }
        public char UptimeGrade { get; set; } = 'F';

        public void SelectFromDb(string nameName)
        {
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();
                using (SqlCommand command =
                    new SqlCommand($"SELECT * FROM Nodes WHERE Name='{nameName}'", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Name = DBUtills.SafeGetString(reader, 1);
                            RectorVersion = DBUtills.SafeGetString(reader, 2);
                            AnydeskID = reader.GetValue(reader.GetOrdinal("AnydeskID")).ToString();
                            NetworkLogin = reader.GetValue(reader.GetOrdinal("NetworkLogin")).ToString();
                            NetworkPassword = reader.GetValue(reader.GetOrdinal("NetworkPassword")).ToString();
                            TVID = reader.GetValue(reader.GetOrdinal("TVID")).ToString();
                            TVPassword = reader.GetValue(reader.GetOrdinal("TVPassword")).ToString();
                            Is64bit = Boolean.Parse(reader.GetValue(reader.GetOrdinal("64Bit")).ToString());
                            OS = reader.GetValue(reader.GetOrdinal("OS")).ToString();
                            RAM = Double.Parse(reader.GetValue(reader.GetOrdinal("RAM")).ToString());
                            Storage = Double.Parse(reader.GetValue(reader.GetOrdinal("Storage")).ToString());
                            MemoryStick = Boolean.Parse(reader.GetValue(reader.GetOrdinal("MemoryStick")).ToString());
                            LocalAddress1 = reader.GetValue(reader.GetOrdinal("LocalAddress1")).ToString();
                            LocalAddress2 = reader.GetValue(reader.GetOrdinal("LocalAddress2")).ToString();
                            Service = reader.GetValue(reader.GetOrdinal("Service")).ToString();
                            Hub = reader.GetValue(reader.GetOrdinal("Hub")).ToString();
                            NodeNumber = Int32.Parse(reader.GetValue(reader.GetOrdinal("NodeNumber")).ToString());
                            PhoneNumberID = reader.GetValue(reader.GetOrdinal("PhoneNumberID")).ToString();
                            Debug.WriteLine(PhoneNumberID);
                            IsSSD = Boolean.Parse(reader.GetValue(reader.GetOrdinal("IsSSD")).ToString());
                            Modem =reader.GetValue(reader.GetOrdinal("Modem")).ToString();
                            NetworkAdapter = Boolean.Parse(reader.GetValue(reader.GetOrdinal("NetworkAdapter")).ToString());
                            GetUptime();
                            GetDataUsage();
                        }
                    }

                    reader.Close();
                }
            }
        }

        private void GetUptime()
        {
            if (Name == "") return;
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            List<double> z = new List<double>();
            List<string> date = new List<string>();
            int tmp;
            double tempServInternet;
            double tempMainInternet;
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
                        new SqlCommand($"SELECT COUNT(ServiceInternet) From NodesUptime WHERE NodeName='{Name}'AND ServiceInternet=1 and Date BETWEEN '{from.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{to.ToString("yyyy-MM-dd HH:mm:ss")}' ", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tmp = reader.GetInt32(0);
                                tempServInternet = (tmp * 100 / 288);
                                x.Add(tempServInternet);
                            }
                        }
                        reader.Close();
                        tmp = 0;
                        command.CommandText =
                            $"SELECT COUNT(ServiceInternet) From NodesUptime WHERE NodeName='{Name}'AND ServiceInternet=0 and Date BETWEEN '{from}' AND '{to}' ";

                        reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tmp = reader.GetInt32(0);
                                tempMainInternet = (tmp * 100 / 288);
                                y.Add(tempMainInternet);
                            }
                        }
                        reader.Close();
                    }
                    date.Add(to.ToString("M/d"));
                    from = from.AddDays(1);
                    to = to.AddDays(1);
                }
            }

            double grade = 0;
            var numbersAndWords = x.Zip(y, (n, w) => new { Serv = n, Main = w });
            foreach (var nw in numbersAndWords)
            {
                z.Add((100 - nw.Serv - nw.Main));
                grade += nw.Main;
            }
            grade = Math.Round(grade / y.Count);
            ServiceInternet = x;
            MainInternet = y;
            DownTime = z;
            DatesUptime = date;
            UptimeGrade = convertToLetterGrade(grade);
        }
        private void GetDataUsage()
        {
            if (PhoneNumberID == "") return;
            List<double> dailyUsage = new List<double>();
            List<string> date = new List<string>();
            double temp1;
            double totalUsed = 0;
            DateTime temp2;
            DateTime to = DateTime.Now;
            DateTime from = to.AddDays(-30);
            using (SqlConnection connection = new SqlConnection(Config.connection_string))
            {
                connection.Open();
                Debug.WriteLine($"Phone ID---{PhoneNumberID}---");
                using (SqlCommand command =
                    new SqlCommand($"SELECT Usage, Date From UsageHistory WHERE ID='{PhoneNumberID}' and Date BETWEEN '{from.ToString("yyyy-MM-dd HH:mm:ss")}' AND '{to.ToString("yyyy-MM-dd HH:mm:ss")}' ", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            temp1 = reader.GetDouble(0);
                            temp2 = reader.GetDateTime(1);

                            dailyUsage.Add(temp1);
                            date.Add(temp2.ToString("M/d"));
                        }
                    }
                    reader.Close();
                }
            }
            List<double> dailyUsageFinal = new List<double>();

            if (dailyUsage.Capacity != 0)
            {
                totalUsed = Math.Round(dailyUsage[dailyUsage.Capacity - 1], 2);
                int i;
                for (i = 0; i < dailyUsage.Capacity - 1; i++)
                {
                    dailyUsageFinal.Add(Math.Round((dailyUsage[i + 1] - dailyUsage[i]), 2));

                }

            }

            DatesUsage = date;
            Usage = dailyUsageFinal;
            TotalUsed = totalUsed;
        }
        private static char convertToLetterGrade(double numberGrade)
        {
            char letter = 'F';
            if (numberGrade >= 90 && numberGrade <= 100)
            {
                letter = 'A';
            }

            else if (numberGrade >= 80 && numberGrade < 90)
            {
                letter = 'B';
            }
            else if (numberGrade >= 70 && numberGrade < 80)
            {
                letter = 'C';
            }
            else if (numberGrade >= 60 && numberGrade < 70)
            {
                letter = 'D';
            }
            return letter;
        }
    }
}
