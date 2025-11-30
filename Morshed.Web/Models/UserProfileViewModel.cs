using System;

namespace Morshed.Web.Models
{
    public class UserProfileViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile? ProfileImage { get; set; }
        public int SavedPlacesCount { get; set; }
        public int VisitedPlacesCount { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
