using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouseweb2.Models
{
   [Table("incoming")]
    public class Incoming
    {
        [Key]
        [Column("incoming_id")]
        public int IncomingId { get; set; }

        [Required]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("expected_quantity")]
        public int ExpectedQuantity { get; set; }

        [Required]
        [Column("expected_arrival_date")]
        [DataType(DataType.Date)] 
        public DateOnly ExpectedArrivalDate { get; set; } 

        [Required]
        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [Column("order_date")]
        [DataType(DataType.Date)]
        public DateOnly? OrderDate { get; set; } 

        [Column("supplier_name")]
        [MaxLength(255)]
        public string? SupplierName { get; set; }

        [Column("destination_location_id")]
        public int? DestinationLocationId { get; set; } 

     
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("DestinationLocationId")]
        public Location? DestinationLocation { get; set; } 
    }
}