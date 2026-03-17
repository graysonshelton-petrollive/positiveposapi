using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSitive.API.Models
{
    [Table("POSFuelPrices_History", Schema = "dbo")]
    public class POSFuelPricesHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableID { get; set; }
        public int CompanyID { get; set; }
        public int SiteID { get; set; }
        public Guid LocationGUID { get; set; }
        [MaxLength(10)]
        public string FuelAbbreviation { get; set; }
        [MaxLength(10)]
        public string NACSCode { get; set; }
        [MaxLength(10)]
        public string NACSCategory { get; set; }
        [Column(TypeName = "decimal(10, 5)")]
        public decimal? FuelPrice { get; set; }
        public DateTime? DateAdded { get; set; }
        [MaxLength(10)]
        public string AddedBy { get; set; }
        public DateTime? DateModified { get; set; }
        [MaxLength(50)]
        public string ModifiedBy { get; set; }
    }
}
