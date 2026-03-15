using System;
using System.Collections.Generic;

namespace PositivePOSAPI.Models;

public partial class UserPermission
{
    public Guid UserPermissionGuid { get; set; }

    public Guid UserId { get; set; }

    public Guid PageGuid { get; set; }

    public bool CanView { get; set; }

    public bool CanEdit { get; set; }

    public DateTime CreatedUtc { get; set; }

    public Guid? CreatedByUserId { get; set; }

    public DateTime ModifiedUtc { get; set; }

    public Guid? ModifiedByUserId { get; set; }

    public virtual AppPage Page { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
