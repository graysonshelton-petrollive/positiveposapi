using System;
using System.Collections.Generic;

namespace PositivePOSAPI.Models;

public partial class Company
{
    public Guid CompanyGuid { get; set; }

    public int? CompanyNumber { get; set; }

    public string Name { get; set; } = null!;

    public string? LegalName { get; set; }

    public string? TaxId { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? ContactNameFirst { get; set; }

    public string? ContactNameLast { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }

    public Guid? CreatedByUserId { get; set; }

    public DateTime ModifiedUtc { get; set; }

    public Guid? ModifiedByUserId { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
