using System;
using System.Collections.Generic;

namespace PositivePOSAPI.Models;

public partial class AppPage
{
    public Guid PageGuid { get; set; }

    public string PageKey { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
