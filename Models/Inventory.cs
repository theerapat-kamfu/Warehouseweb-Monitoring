using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouseweb2.Models
{
    [Table("inventory")]
    public class Inventory
    {
        [Key]
        [Column("inventory_id")]
        public int InventoryId { get; set; }

        [Required]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("location_id")]
        public int LocationId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; } = 0;

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow; 


        [ForeignKey("ProductId")]
        public Product? Product { get; set; } 

        [ForeignKey("LocationId")]
        public Location? Location { get; set; } 
    }

}