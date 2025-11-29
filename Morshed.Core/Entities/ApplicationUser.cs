using Microsoft.AspNetCore.Identity;
using System;

namespace Morshed.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
