namespace Morshed.Core.Entities
{
    public class TourStop
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public int DayNumber { get; set; }
        public int OrderIndex { get; set; }
    }
}
