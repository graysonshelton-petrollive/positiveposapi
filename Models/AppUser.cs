using Microsoft.AspNetCore.Identity;

namespace PositivePOSAPI.Models;

public class AppUser : IdentityUser<Guid>
{
    public string Discriminator { get; set; } = "ApplicationUser";

    public string? NamePrefix { get; set; }
    public string NameFirst { get; set; } = string.Empty;
    public string? NameMiddle { get; set; }
    public string NameLast { get; set; } = string.Empty;
    public string? NameSuffix { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateLastLogin { get; set; }

    public int CompanyId { get; set; }
    public int LocationId { get; set; }

    public bool? MustResetPassword { get; set; }
}