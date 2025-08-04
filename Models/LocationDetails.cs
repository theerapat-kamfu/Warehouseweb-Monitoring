using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouseweb2.Models
{
    public class LocationDetails
    {
        public int location_id { get; set; }
        public string location_name { get; set; }
        public int quantity { get; set; }
        public int capacity { get; set; }
        public string unit { get; set; }
    }
}