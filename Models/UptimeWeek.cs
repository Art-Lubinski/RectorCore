using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RectorCore.Models
{
    public class UptimeWeek
    {
        public List<double> ServiceInternet { get; set; }
        public List<double> MainInternet { get; set; }
        public List<double> DownTime { get; set; }
        public List<string> Dates { get; set; }
    }
}
