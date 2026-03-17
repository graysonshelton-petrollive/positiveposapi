using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace POSitive.API.Models
{
    [Table("POSTransactionsDetailFuel", Schema = "dbo")]
    [PrimaryKey(nameof(CompanyID), nameof(SiteID), nameof(LocationGUID), nameof(InvoiceNumber), nameof(LineNumber))]
    public class POSTransactionsDetailFuel
    {
        // Composite FK back to POSTransactionsDetail (and also PK for this table)
        public int CompanyID { get; set; }
        public int SiteID { get; set; }
        public Guid LocationGUID { get; set; }
        public int InvoiceNumber { get; set; }
        public int LineNumber { get; set; }

        // Fuel specifics
        public int? DispenserNumber { get; set; }
        public int? HoseNumber { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string? GradeCode { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string? FuelProductCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? ControllerTxnId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? AuthorizationId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? SaleType { get; set; } // PREAUTH, CAPTURE, PREPAY, etc.

        [Column(TypeName = "decimal(12, 3)")]
        public decimal? VolumeGallons { get; set; }

        [Column(TypeName = "decimal(12, 4)")]
        public decimal? UnitPrice { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal? ExtendedAmount { get; set; }

        [Column(TypeName = "decimal(12, 3)")]
        public decimal? TaxAmount { get; set; }

        // Tracking
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedUtc { get; set; }

        // Navigation back to the line item
        // public POSTransactionsDetail? Detail { get; set; }
    }
}
