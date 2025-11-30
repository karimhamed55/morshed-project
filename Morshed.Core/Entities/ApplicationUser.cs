using Microsoft.AspNetCore.Identity;
using System;

namespace Morshed.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            FirstName = "";
            LastName = "";
            Country = "";
            Gender = "";
            Bio = "Hi! I am a traveler.";
            ProfilePictureUrl = "/images/default_user.png";
            Age = 18;
            SavedPlacesCount = 0;
            VisitedPlacesCount = 0;
        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Profile Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int SavedPlacesCount { get; set; }
        public int VisitedPlacesCount { get; set; }
    }
}
