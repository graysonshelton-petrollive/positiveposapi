using System;
using System.Collections.Generic;

namespace PositivePOSAPI.Models;

public partial class Location
{
    public Guid LocationGuid { get; set; }

    public Guid CompanyGuid { get; set; }

    public int LocationNumber { get; set; }

    public string Name { get; set; } = null!;

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? Timezone { get; set; }

    public bool IsHQ { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }

    public Guid? CreatedByUserId { get; set; }

    public DateTime ModifiedUtc { get; set; }

    public Guid? ModifiedByUserId { get; set; }

    public virtual Company Company { get; set; } = null!;
}
