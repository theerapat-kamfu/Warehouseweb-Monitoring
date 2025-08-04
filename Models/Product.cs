using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouseweb2.Models
{
    [Table("product")] 
    public class Product
    {
        [Key] 
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required] 
        [Column("product_name")]
        [MaxLength(255)] 
        public string ProductName { get; set; } = string.Empty; 

        [Column("category")]
        [MaxLength(100)]
        public string? Category { get; set; } 

        [Required]
        [Column("sku")]
        [MaxLength(50)]
        public string Sku { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("weight", TypeName = "decimal(10, 2)")] 
        public decimal? Weight { get; set; }

        [Column("volume", TypeName = "decimal(10, 2)")]
        public decimal? Volume { get; set; }

        [Column("reorder_level")]
        public int ReorderLevel { get; set; } = 0; 

        
        public ICollection<Inventory>? Inventories { get; set; }
  
        public ICollection<Incoming>? Incomings { get; set; }
  
        public ICollection<Outgoing>? Outgoings { get; set; }
    }
}