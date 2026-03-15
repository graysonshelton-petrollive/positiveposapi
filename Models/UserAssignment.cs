using System;
using System.Collections.Generic;

namespace PositivePOSAPI.Models;

public partial class UserAssignment
{
    public Guid UserAssignmentGuid { get; set; }

    public Guid UserId { get; set; }

    public Guid CompanyGuid { get; set; }

    public Guid? LocationGuid { get; set; }

    public bool IsPrimary { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }

    public Guid? CreatedByUserId { get; set; }

    public DateTime ModifiedUtc { get; set; }

    public Guid? ModifiedByUserId { get; set; }
}
