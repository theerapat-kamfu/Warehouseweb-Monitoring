using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouseweb2.Models
{
    public class Top5Product
    {
        public string category { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public string location_name { get; set; }
        public string last_update { get; set; }

    }
}