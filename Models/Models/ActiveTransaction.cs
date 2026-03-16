using System;
using System.ComponentModel.DataAnnotations;

namespace POSitive.API.Models
{
    public class ActiveTransaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid LocationGUID { get; set; }

        public int? CompanyId { get; set; }
        public int? SiteID { get; set; }

        public string? RegisterId { get; set; }

        public int PumpId { get; set; }
        public int? HoseId { get; set; }

        public int? InvoiceNumber { get; set; }

        public decimal Gallons { get; set; } = 0m;
        public decimal Amount { get; set; } = 0m;

        public string? FuelType { get; set; }
        public decimal? UnitPrice { get; set; }

        // Active | Completed | Voided
        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAtUtc { get; set; }
    }
}
