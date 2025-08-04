using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouseweb2.Models
{
    public class OutgoingItem
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Category { get; set; }
        public string? CustomerName { get; set; }
        public int OrderedQuantity { get; set; } 
        public string? ScheduledShipDate { get; set; }
        public string Status { get; set; }
    }
}