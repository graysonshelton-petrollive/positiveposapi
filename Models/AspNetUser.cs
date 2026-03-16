using System;
using System.Collections.Generic;
// susing FOisutuveAPI.Models;

namespace PositivePOSAPI.Models;

public partial class AspNetUser
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public int? CompanyID { get; set; }

    public int? LocationID { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateLastLogin { get; set; }

    public byte? MustResetPassword { get; set; }

    public string? NameFirst { get; set; }

    public string? NameLast { get; set; }

    public string? NameMiddle { get; set; }

    public string? NamePrefix { get; set; }

    public string? NameSuffix { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual UserApproval? UserApproval { get; set; }

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
