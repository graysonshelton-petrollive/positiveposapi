using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POSitive.API.Models
{
    [Table("POSFuelPrices", Schema = "dbo")]
    public class POSFuelPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableID { get; set; }

        [Required]
        public int CompanyID { get; set; }

        [Required]
        public int SiteID { get; set; }

        [Required]
        public Guid LocationGUID { get; set; }

        [MaxLength(10)]
        public string FuelAbbreviation { get; set; }

        [MaxLength(10)]
        public string NACSCode { get; set; }

        [MaxLength(10)]
        public string NACSCategory { get; set; }

        [Column(TypeName = "decimal(10, 5)")]
        public decimal FuelPrice { get; set; }

        public DateTime? DateAdded { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? AddedBy { get; set; }

        public DateTime? DateModified { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string? ModifiedBy { get; set; }
    }
}

namespace POSitive.API.Models
{
    [Table("FuelType", Schema = "Reference")]
    public class FuelType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableID { get; set; }

        [Required]
        public string Abbreviation { get; set; } // Example: "UNLEA", "DSL"
        [MaxLength(10)]
        public string NACSCode { get; set; } // NACS Code
        [MaxLength(10)]
        public string NACSCategory { get; set; } // NACS Category
    }
}
