using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace POSitive.API.Models
{
    [Table("POSTransaction", Schema = "dbo")]

    [PrimaryKey(nameof(CompanyID), nameof(SiteID), nameof(LocationGUID), nameof(InvoiceNumber))]

    public class POSTransaction
    {
        // Composite Primary Key: LocationGUID and InvoiceNumber
        public Guid LocationGUID { get; set; }

        public int InvoiceNumber { get; set; }

        [Required]
        public int CompanyID { get; set; }

        [Required]
        public int SiteID { get; set; }

        public DateTime? OrderDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalAmount { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? SalesTax { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalTax { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Subtotal { get; set; }
        

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? AmountPaid { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? AmountBalance { get; set; }

        [MaxLength(20)]
        public string? CheckNumber { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? CashAmount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? CheckAmount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? CreditAmount { get; set; }

        [MaxLength(50)]
        public string? UserID { get; set; }

        [MaxLength(2)]
        public string? Terminal { get; set; }

        [MaxLength(20)]
        public string? PaymentType { get; set; }

        [MaxLength(10)]
        public string OrderStatus { get; set; }

        // Newly Added Fields

        [MaxLength(25)]
        public string? CardType { get; set; }

        public int? First6 { get; set; }

        public int? Last4 { get; set; }

        public int? CardExpirationMM { get; set; }

        public int? CardExpirationYY { get; set; }

        public int? ShiftID { get; set; }
        
        [MaxLength(10)]
        public string? ReceiptType { get; set; }

        [Column(TypeName = "text")]
        public string? CardTrack2 { get; set; }

        [Column(TypeName = "text")]
        public string? CardResponse { get; set; }
    }
}
