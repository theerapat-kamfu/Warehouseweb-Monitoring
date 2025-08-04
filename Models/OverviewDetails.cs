using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouseweb2.Models
{
    public class OverviewDetails
    {
        public int total { get; set; }
        public int capacity { get; set; }
        public double usage { get; set; }
        public int incoming { get; set; }
        public int outgoing { get; set; }


    }
}