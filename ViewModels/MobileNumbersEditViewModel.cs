using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RectorCore.Models;

namespace RectorCore.ViewModels
{
    public class MobileNumbersEditViewModel
    {
        public string number { get; set; }
        public string plan { get; set; }
        public string numberId { get; set; }
        public string provider { get; set; }
        public string accountId { get; set; }
        public string accountNumber { get; set; }
        public string nodeName { get; set; }
        public List<string> accounts { get; set; }
        public List<string> providers { get; set; }
    }
}
