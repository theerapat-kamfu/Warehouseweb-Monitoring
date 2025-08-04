using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouseweb2.Models
{
     [Table("outgoing")]
    public class Outgoing
    {
        [Key]
        [Column("outgoing_id")]
        public int OutgoingId { get; set; }

        [Required]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("ordered_quantity")]
        public int OrderedQuantity { get; set; }

        [Required]
        [Column("scheduled_ship_date")]
        [DataType(DataType.Date)]
        public DateOnly ScheduledShipDate { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [Column("order_date")]
        [DataType(DataType.Date)]
        public DateOnly? OrderDate { get; set; }

        [Column("customer_name")]
        [MaxLength(255)]
        public string? CustomerName { get; set; }

        [Column("source_location_id")]
        public int? SourceLocationId { get; set; } 

    
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("SourceLocationId")]
        public Location? SourceLocation { get; set; } 
    }
}