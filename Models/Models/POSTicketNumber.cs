using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace POSitive.API.Models
{
    [Table("POSTicketNumbers", Schema = "dbo")]
    [PrimaryKey(nameof(LocationGUID), nameof(TicketNumber))] // Matches SQL PRIMARY KEY
    public class POSTicketNumber
    {
        [Required]
        public int CompanyID { get; set; }

        [Required]
        public int SiteID { get; set; }

        public Guid LocationGUID { get; set; }

        public int TicketNumber { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }
    }
}
