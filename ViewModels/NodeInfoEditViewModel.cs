using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RectorCore.ViewModels
{
    public class NodeInfoEditViewModel
    {
        public string Status { get; set; }
        public string AnydeskPassword { get; set; }
        public string TVPassword { get; set; }
        public string PhoneNumberID { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public List<string> PhoneNumbersIDS { get; set; }
        

    }
}
