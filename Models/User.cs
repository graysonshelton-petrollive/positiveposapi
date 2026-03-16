using Microsoft.AspNetCore.Identity;
using System;

namespace POSitive.API.Models
{
    public class User : IdentityUser<string>
    {
        // Add your custom fields here
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Role { get; set; }  // Add Role Field

        // You can add navigation properties for relationships here if needed
    }
}

