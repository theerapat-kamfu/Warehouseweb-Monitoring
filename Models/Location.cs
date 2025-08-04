using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouseweb2.Models
{
    [Table("location")]
    public class Location
    {
        [Key]
        [Column("location_id")]
        public int LocationId { get; set; }

        [Required]
        [Column("location_name")]
        [MaxLength(255)]
        public string LocationName { get; set; } = string.Empty;

        [Column("capacity", TypeName = "decimal(12, 2)")]
        public decimal? Capacity { get; set; }

        [Column("unit_of_capacity")]
        [MaxLength(50)]
        public string? UnitOfCapacity { get; set; }

        [Column("location_type")]
        [MaxLength(100)]
        public string? LocationType { get; set; }

        [Column("address")]
        public string? Address { get; set; }


        public ICollection<Inventory>? Inventories { get; set; }

        public ICollection<Incoming>? DestinationIncomings { get; set; }
  
        public ICollection<Outgoing>? SourceOutgoings { get; set; }
    }
}