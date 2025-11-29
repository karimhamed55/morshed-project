using System;

namespace Morshed.Core.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int Rating { get; set; } // 1-5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
