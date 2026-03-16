using Microsoft.AspNetCore.Identity;

namespace POSitive.API.Models
{
    public class AppRole : IdentityRole
    {
        // Only keep this if dbo.AspNetRoles actually has this column
        public string? Description { get; set; }
    }
}
