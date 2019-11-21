using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Int32 PhoneNumberID { get; set; }
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
    }
}
