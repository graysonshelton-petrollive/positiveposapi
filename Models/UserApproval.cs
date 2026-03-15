using System;
using System.Collections.Generic;

namespace PositivePOSAPI.Models;

public partial class UserApproval
{
    public Guid UserId { get; set; }

    public bool IsApproved { get; set; }

    public DateTime? ApprovedUtc { get; set; }

    public Guid? ApprovedByUserId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedUtc { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
}
