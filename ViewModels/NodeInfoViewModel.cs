using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RectorCore.ViewModels
{
    public class NodeInfoViewModel
    {
        public string Status { get; set; }
        public string OS { get; set; }
        public double RAM { get; set; }
        public string Modem { get; set; }
        public double Storage { get; set; }
        public bool MemoryStick { get; set; }
        public string LocalAddress1 { get; set; }
        public string LocalAddress2 { get; set; }
        public Int32 UptimeDay { get; set; }
        public Int32 UptimeWeek { get; set; }
    }
}
