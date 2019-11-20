using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RectorCore.Models
{
    public class DailyUsage
    {
        public List<double> Usage { get; set; }
        public List<string> Dates { get; set; }
    }
}
