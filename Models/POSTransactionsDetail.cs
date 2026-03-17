using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace POSitive.API.Models
{
    [Table("POSTransactionsDetail", Schema = "dbo")]
    [PrimaryKey(nameof(CompanyID), nameof(SiteID), nameof(LocationGUID), nameof(InvoiceNumber), nameof(LineNumber))]
    public class POSTransactionsDetail
    {
        // Composite Key Columns
        public int CompanyID { get; set; }
        public int SiteID { get; set; }

        [Required]
        public Guid LocationGUID { get; set; }

        public int InvoiceNumber { get; set; }
        public int LineNumber { get; set; }

        // Core Line Item Fields
        [Required]
        [MaxLength(50)]
        public string ItemID { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ItemDescription { get; set; }

        [Column(TypeName = "decimal(10, 3)")]
        public decimal? ItemPrice { get; set; }

        [Column(TypeName = "decimal(10, 3)")]
        public decimal? LineItemTaxAmount { get; set; }

        [Column(TypeName = "decimal(10, 3)")]
        public decimal? Quantity { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalPrice { get; set; }

        public int? TaxableInd { get; set; }

        // Fuel-ish flags (compat)
        public int? FuelPrepayInd { get; set; }
        public int? FuelFillupInd { get; set; }
        public int? DispenserNumber { get; set; }
        public int? PriceOverride { get; set; }

        [Column(TypeName = "decimal(10, 3)")]
        public decimal? NonSalePrice { get; set; }

        public int? SpecialInd { get; set; }

        // Retail / Program Fields
        [MaxLength(50)]
        public string? CategoryCode { get; set; }

        public int? SafeDropInd { get; set; }
        public int? DiscountInd { get; set; }

        [MaxLength(50)]
        public string? DiscountCode { get; set; }

        public int? LoyaltyInd { get; set; }

        [MaxLength(50)]
        public string? LoyaltyCode { get; set; }

        // Optional 1-to-1 fuel detail table
        // public POSTransactionsDetailFuel? Fuel { get; set; }

        // // Convenience helpers (no schema impact)
        // [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        // public bool IsFuelLike =>
        //     Fuel != null ||
        //     (FuelPrepayInd ?? 0) != 0 ||
        //     (FuelFillupInd ?? 0) != 0 ||
        //     (DispenserNumber ?? 0) > 0;
    }
}
