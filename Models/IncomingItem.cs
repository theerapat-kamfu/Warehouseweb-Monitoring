using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouseweb2.Models
{
    public class IncomingItem
    {
        public int ProductId { get; set; } 
        public string ProductName { get; set; } 
        public string Category { get; set; } 
        public string SupplierName { get; set; } 
        public int ExpectedQuantity { get; set; } 
        public string ExpectedArrivalDate { get; set; } 
        public string Status { get; set; } 

    }
}