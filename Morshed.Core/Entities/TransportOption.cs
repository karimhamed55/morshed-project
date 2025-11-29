using System;

namespace Morshed.Core.Entities
{
    public class TransportOption
    {
        public int Id { get; set; }
        public int PlaceFromId { get; set; }
        public Place PlaceFrom { get; set; }

        public int PlaceToId { get; set; }
        public Place PlaceTo { get; set; }

        public string Type { get; set; } // e.g., Taxi, Bus, Walk
        public decimal Cost { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
