using System.Collections.Generic;

namespace Morshed.Core.Entities
{
    public class Place
    {
        public int Id { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }

        public string Name { get; set; }
        public string Category { get; set; } // e.g., Temple, Museum, Restaurant
        public string Address { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Description { get; set; }

        public ICollection<PlaceImage> Images { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
